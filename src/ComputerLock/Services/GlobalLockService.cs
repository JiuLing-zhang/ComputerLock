﻿using ComputerLock.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Win32;

namespace ComputerLock.Services;

/// <summary>
/// 锁定器，全局锁定：包括鼠标、键盘、任务管理器、系统快捷键等
/// </summary>
internal class GlobalLockService : IGlobalLockService
{
    private readonly ILogger _logger;
    private readonly AppSettings _appSettings;
    private IScreenLockService? _screenLockService;
    private UserActivityMonitor? _activityMonitor;
    private readonly HotkeyHook _hotkeyHook;
    private readonly TaskManagerHook _taskManagerHook;
    private readonly MouseHook _mouseHook;
    private readonly SystemKeyHook _systemKeyHook;
    private readonly IServiceProvider _serviceProvider;
    private readonly IWindowsMessageBox _messageBox;
    private readonly IStringLocalizer<Lang> _lang;
    public bool IsLocked { get; private set; }
    private CancellationTokenSource? _cts;
    public GlobalLockService(ILogger logger, AppSettings appSettings, UserActivityMonitor activityMonitor, HotkeyHook hotkeyHook, TaskManagerHook taskManagerHook, MouseHook mouseHook, SystemKeyHook systemKeyHook, IServiceProvider serviceProvider, IWindowsMessageBox messageBox, IStringLocalizer<Lang> lang)
    {
        _logger = logger;
        _appSettings = appSettings;

        InitActivityMonitor(activityMonitor);
        _hotkeyHook = hotkeyHook;
        _taskManagerHook = taskManagerHook;

        // 防止锁屏时系统崩溃、重启等问题导致任务栏被禁用
        // 启动时默认启用一次        
        // TODO 这里需要重新设计，不应该在这里启用任务管理器，因为有可能任务管理器是被其他程序主动禁用的
        _taskManagerHook.EnabledTaskManager();
        _mouseHook = mouseHook;
        _systemKeyHook = systemKeyHook;
        _serviceProvider = serviceProvider;
        _messageBox = messageBox;
        _lang = lang;
    }

    /// <summary>
    /// 初始化空闲检测
    /// </summary>
    private void InitActivityMonitor(UserActivityMonitor userActivityMonitor)
    {
        if (_appSettings.AutoLockSecond <= 0)
        {
            return;
        }
        _activityMonitor = userActivityMonitor;

        _logger.Write("空闲自动锁定已生效");
        _activityMonitor.Init(_appSettings.AutoLockSecond);
        _activityMonitor.OnIdle += (_, _) =>
        {
            _logger.Write("空闲自动锁定 -> 执行空闲锁定");
            Lock();
        };
        _logger.Write("空闲自动锁定 -> 启动空闲检测");
        _activityMonitor.StartMonitoring();

        _logger.Write("空闲自动锁定 -> 准备监控系统会话状态");
        SystemEvents.SessionSwitch += (_, e) =>
        {
            if (e.Reason == SessionSwitchReason.SessionLock)
            {
                _logger.Write("空闲自动锁定 -> Windows系统锁定，暂停空闲检测");
                _activityMonitor.StopMonitoring();
            }
            else if (e.Reason == SessionSwitchReason.SessionUnlock)
            {
                _logger.Write("空闲自动锁定 -> Windows系统解锁，恢复空闲检测");
                _activityMonitor.StartMonitoring();
            }
        };
    }

    public void Lock()
    {
        if (!CheckLockConfig(out var message))
        {
            _messageBox.Show(message);
            return;
        }

        _screenLockService = _serviceProvider.GetRequiredKeyedService<IScreenLockService>(_appSettings.ScreenUnlockMethod);

        if (!_screenLockService.Lock(_appSettings.LockAnimation))
        {
            _logger.Write("全局锁定 -> 屏幕锁定失败");
            return;
        }

        if (_appSettings.ScreenUnlockMethod == ScreenUnlockMethods.Password)
        {
            _screenLockService.OnUnlock += _screenLockService_OnUnlock;
        }

        _logger.Write("全局锁定 -> 暂停空闲检测");
        _activityMonitor?.StopMonitoring();

        _logger.Write("全局锁定 -> 禁用任务管理器和系统键");
        _taskManagerHook.DisabledTaskManager();

        if (_appSettings.IsHideMouseCursor)
        {
            _logger.Write("全局锁定 -> 隐藏鼠标光标");
            _mouseHook.HideCursor();
        }

        if (_appSettings.ScreenUnlockMethod == ScreenUnlockMethods.Hotkey)
        {
            _logger.Write("全局锁定 -> 允许快捷键解锁，准备放行快捷键");
            if (_appSettings.IsUnlockUseLockHotkey)
            {
                if (_appSettings.LockHotkey != null)
                {
                    _systemKeyHook.SetIgnoreHotkey(_appSettings.LockHotkey);
                }
            }
            else
            {
                if (_appSettings.UnlockHotkey != null)
                {
                    _systemKeyHook.SetIgnoreHotkey(_appSettings.UnlockHotkey);
                }
            }
        }
        _systemKeyHook.DisableSystemKey();

        if (_appSettings.IsDisableWindowsLock)
        {
            _cts = new CancellationTokenSource();
            _logger.Write("全局锁定 -> 禁用 Windows 锁屏");
            Task.Run(async () =>
            {
                while (true)
                {
                    _logger.Write("全局锁定 -> 移动鼠标，防止 Windows 锁屏");
                    _mouseHook.MoveAndClick();
                    try
                    {
                        await Task.Delay(TimeSpan.FromSeconds(30), _cts.Token);
                    }
                    catch (TaskCanceledException)
                    {
                        _logger.Write("全局锁定 -> 鼠标任务释放");
                        break;
                    }
                }
            }, _cts.Token);
        }
        IsLocked = true;
    }

    private bool CheckLockConfig(out string error)
    {
        if (_appSettings.ScreenUnlockMethod == ScreenUnlockMethods.Hotkey)
        {
            if (_appSettings.IsUnlockUseLockHotkey)
            {
                if (_appSettings.LockHotkey == null)
                {
                    error = _lang["LockHotkeyUndefined"];
                    return false;
                }
            }
            else
            {
                if (_appSettings.UnlockHotkey == null)
                {
                    error = _lang["UnlockHotkeyUndefined"];
                    return false;
                }
            }
        }
        error = "";
        return true;
    }

    /// <summary>
    /// 解锁（目前只有快捷键锁屏才会显式调用）
    /// </summary>
    public void Unlock()
    {
        _screenLockService!.Unlock();
        SystemUnlock();
        IsLocked = false;
    }

    private void _screenLockService_OnUnlock(object? sender, EventArgs e)
    {
        _screenLockService!.OnUnlock -= _screenLockService_OnUnlock;
        SystemUnlock();
        IsLocked = false;
    }

    /// <summary>
    /// 系统层面解锁（不包括本程序窗口）
    /// </summary>
    private void SystemUnlock()
    {
        _logger.Write("系统锁定 -> 启动空闲检测");
        _activityMonitor?.StartMonitoring();

        _logger.Write("系统锁定 -> 恢复任务管理器和系统键");
        _taskManagerHook.EnabledTaskManager();

        if (_appSettings.IsHideMouseCursor)
        {
            _logger.Write("系统锁定 -> 恢复鼠标光标");
            _mouseHook.ResetCursorState();
        }

        _systemKeyHook.Dispose();

        if (_cts != null)
        {
            _cts.Cancel();
            _cts.Dispose();
            _cts = null;
        }
    }

    public void Dispose()
    {
        _hotkeyHook.Dispose();
        _activityMonitor?.Dispose();
    }
}