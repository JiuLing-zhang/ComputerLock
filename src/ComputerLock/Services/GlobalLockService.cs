using ComputerLock.Interfaces;
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
    private readonly UserActivityMonitor _activityMonitor;
    private readonly HotkeyHook _hotkeyHook;
    private readonly TaskManagerHook _taskManagerHook;
    private readonly MouseHook _mouseHook;
    private readonly SystemKeyHook _systemKeyHook;
    private readonly IServiceProvider _serviceProvider;
    private readonly IWindowsMessageBox _messageBox;
    private readonly IStringLocalizer<Lang> _lang;
    private readonly PopupService _popupService;
    public bool IsLocked { get; private set; }
    private bool _isWindowsLocked;
    private CancellationTokenSource? _cts;
    private CancellationTokenSource? _powerActionCts;
    private PowerManager _powerManager;
    public GlobalLockService(ILogger logger, AppSettings appSettings, UserActivityMonitor activityMonitor, HotkeyHook hotkeyHook, TaskManagerHook taskManagerHook, MouseHook mouseHook, SystemKeyHook systemKeyHook, IServiceProvider serviceProvider, IWindowsMessageBox messageBox, IStringLocalizer<Lang> lang, PopupService popupService, PowerManager powerManager)
    {
        _logger = logger;
        _appSettings = appSettings;
        _activityMonitor = activityMonitor;
        _hotkeyHook = hotkeyHook;
        _taskManagerHook = taskManagerHook;

        _taskManagerHook.RecoverFromCrash();
        _mouseHook = mouseHook;
        _systemKeyHook = systemKeyHook;
        _serviceProvider = serviceProvider;
        _messageBox = messageBox;
        _lang = lang;
        _popupService = popupService;
        _powerManager = powerManager;

        InitActivityMonitor();
        InitUserInputHandling();

        _logger.Info("空闲自动锁定 -> 准备监控系统会话状态");
        SystemEvents.SessionSwitch += SystemEvents_SessionSwitch;
    }

    /// <summary>
    /// 初始化空闲检测
    /// </summary>
    private void InitActivityMonitor()
    {
        _activityMonitor.OnIdle += (_, _) =>
        {
            _logger.Info("空闲自动锁定 -> 执行空闲锁定");
            Lock();
        };

        AutoLockStart();
    }

    private void InitUserInputHandling()
    {
        _logger.Info("系统 -> 准备监听用户输入状态");
        _systemKeyHook.OnUserInput += (_, _) =>
        {
            if (_appSettings.LockTips && IsLocked)
            {
                _logger.Info("用户输入 -> 检测到键盘输入");
                _popupService.ShowMessage(_lang["LockTipsValue"]);
            }
        };

        _mouseHook.OnUserInput += (_, _) =>
        {
            if (_appSettings.LockTips && IsLocked)
            {
                _logger.Info("用户输入 -> 检测到鼠标输入");
                _popupService.ShowMessage(_lang["LockTipsValue"]);
            }
        };
    }

    /// <summary>
    /// Windows 事件监控
    /// </summary>
    private void SystemEvents_SessionSwitch(object sender, SessionSwitchEventArgs e)
    {
        if (e.Reason == SessionSwitchReason.SessionLock)
        {
            _logger.Info("系统 -> Windows 系统已锁定");
            _isWindowsLocked = true;
            WindowsLock();
        }
        else if (e.Reason == SessionSwitchReason.SessionUnlock)
        {
            _logger.Info("系统 -> Windows 系统已解除锁定");
            _isWindowsLocked = false;
            WindowsUnlock();
        }
    }

    /// <summary>
    /// Windows 操作系统锁定
    /// </summary>
    private void WindowsLock()
    {
        if (!_appSettings.IsUnlockWhenWindowsLock)
        {
            if (!IsLocked)
            {
                _logger.Info("系统 -> Windows 系统锁定，暂停空闲检测");
                _activityMonitor.StopMonitoring();
            }
        }
        else
        {
            if (IsLocked)
            {
                _logger.Info("系统 -> Windows 系统锁定，程序解锁");
                Unlock();
            }
        }
    }

    /// <summary>
    /// Windows 操作系统解锁
    /// </summary>
    private void WindowsUnlock()
    {
        if (!_appSettings.IsUnlockWhenWindowsLock)
        {
            if (!IsLocked)
            {
                _logger.Info($"系统 -> Windows 系统解锁");
                AutoLockStart();
            }
        }
    }

    private void AutoLockStart()
    {
        if (_isWindowsLocked && _appSettings.IsUnlockWhenWindowsLock)
        {
            _logger.Info($"系统 -> Windows 锁定状态，不启用空闲检测");
        }

        if (_appSettings.AutoLockSecond > 0)
        {
            _logger.Info($"系统 -> 启动空闲检测，{_appSettings.AutoLockSecond} 秒");
            _activityMonitor.SetAutoLockSecond(_appSettings.AutoLockSecond);
            _activityMonitor.StartMonitoring();
        }
    }

    public void Lock()
    {
        if (_isWindowsLocked && _appSettings.IsUnlockWhenWindowsLock)
        {
            _logger.Info($"系统 -> Windows 锁定状态禁止程序锁定");
            return;
        }

        if (!CheckLockConfig(out var message))
        {
            _messageBox.Show(message);
            return;
        }

        _screenLockService = _serviceProvider.GetRequiredKeyedService<IScreenLockService>(_appSettings.ScreenUnlockMethod);

        if (!_screenLockService.Lock(_appSettings.LockAnimation))
        {
            _logger.Info("全局锁定 -> 屏幕锁定失败");
            return;
        }

        if (_appSettings.ScreenUnlockMethod == ScreenUnlockMethods.Password)
        {
            _screenLockService.OnUnlock += _screenLockService_OnUnlock;
        }

        _logger.Info("全局锁定 -> 暂停空闲检测");
        _activityMonitor.StopMonitoring();

        _logger.Info("全局锁定 -> 禁用任务管理器和系统键");
        _taskManagerHook.Lock();

        if (_appSettings.IsHideMouseCursor)
        {
            _logger.Info("全局锁定 -> 隐藏鼠标光标");
            _mouseHook.HideCursor();
        }

        _logger.Info("全局锁定 -> 启用鼠标钩子");
        _mouseHook.InstallHook();

        if (_appSettings.ScreenUnlockMethod == ScreenUnlockMethods.Hotkey)
        {
            _logger.Info("全局锁定 -> 允许快捷键解锁，准备放行快捷键");
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
        else if (_appSettings.ScreenUnlockMethod == ScreenUnlockMethods.Password)
        {
            _logger.Info("全局锁定 -> 密码解锁，准备放行非系统按键");
            _systemKeyHook.SetIgnoreHotkey(null);
        }
        _systemKeyHook.InstallHook();

        if (_appSettings.IsDisableWindowsLock)
        {
            _cts = new CancellationTokenSource();
            _logger.Info("全局锁定 -> 禁用 Windows 锁屏");
            Task.Run(async () =>
            {
                while (true)
                {
                    _logger.Info("全局锁定 -> 移动鼠标，防止 Windows 锁屏");
                    _mouseHook.MoveAndClick();
                    try
                    {
                        await Task.Delay(TimeSpan.FromSeconds(55), _cts.Token);
                    }
                    catch (TaskCanceledException)
                    {
                        _logger.Info("全局锁定 -> 鼠标任务释放");
                        break;
                    }
                }
            }, _cts.Token);
        }
        if (_appSettings.AutoPowerSecond > 0)
        {
            _powerActionCts = new CancellationTokenSource();
            _logger.Info($"全局锁定 -> {_appSettings.AutoPowerSecond}秒后自动执行{_appSettings.AutoPowerActionType.GetDescription()}");
            Task.Run(async () =>
            {
                try
                {
                    await Task.Delay(TimeSpan.FromSeconds(_appSettings.AutoPowerSecond), _powerActionCts.Token);
                    if (_appSettings.AutoPowerActionType == PowerActionType.Shutdown)
                    {
                        _logger.Info("准备执行关机");
                        _powerManager.Shutdown();
                    }
                    else if (_appSettings.AutoPowerActionType == PowerActionType.Hibernate)
                    {
                        _logger.Info("准备执行休眠");
                        _powerManager.Hibernate();
                    }
                }
                catch (TaskCanceledException)
                {
                    _logger.Info("自动关机/休眠任务已取消");
                }
                catch (Exception ex)
                {
                    _logger.Error($"自动关机/休眠任务异常", ex);
                }
            }, _powerActionCts.Token);
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
        else if (_appSettings.ScreenUnlockMethod == ScreenUnlockMethods.Password)
        {
            if (_appSettings.Password.IsEmpty())
            {
                error = _lang["UnlockPasswordUndefined"];
                return false;
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

        if (_powerActionCts != null)
        {
            _powerActionCts.Cancel();
            _powerActionCts.Dispose();
            _powerActionCts = null;
        }
        IsLocked = false;

    }

    public void UpdateAutoLockSettings()
    {
        _logger.Info("系统 -> 更新自动锁定设置");
        _activityMonitor.StopMonitoring();
        AutoLockStart();
    }

    private void _screenLockService_OnUnlock(object? sender, EventArgs e)
    {
        _screenLockService!.OnUnlock -= _screenLockService_OnUnlock;
        Unlock();
    }

    /// <summary>
    /// 系统层面解锁（不包括本程序窗口）
    /// </summary>
    private void SystemUnlock()
    {
        AutoLockStart();

        _logger.Info("系统解锁 -> 恢复任务管理器和系统键");
        _taskManagerHook.Unlock();

        if (_appSettings.IsHideMouseCursor)
        {
            _logger.Info("系统解锁 -> 恢复鼠标光标");
            _mouseHook.ResetCursorState();
        }
        _mouseHook.Dispose();
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
        _activityMonitor.Dispose();
        _hotkeyHook.Dispose();
        SystemEvents.SessionSwitch -= SystemEvents_SessionSwitch;
    }
}