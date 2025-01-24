using Microsoft.Win32;

namespace ComputerLock.Platforms;

/// <summary>
/// 锁定器，全局锁定：包括鼠标、键盘、任务管理器、系统快捷键等
/// </summary>
internal class Locker : ILocker
{
    private readonly ILogger _logger;
    private readonly AppSettings _appSettings;
    private readonly LockService _lockService;
    private UserActivityMonitor? _activityMonitor;
    private readonly HotKeyHook _hotKeyHook;
    private readonly TaskManagerHook _taskManagerHook;
    private readonly MouseHook _mouseHook;
    private readonly SystemKeyHook _systemKeyHook;

    public Locker(ILogger logger, AppSettings appSettings, UserActivityMonitor activityMonitor, LockService lockService, HotKeyHook hotKeyHook, TaskManagerHook taskManagerHook, MouseHook mouseHook, SystemKeyHook systemKeyHook)
    {
        _logger = logger;
        _appSettings = appSettings;
        _lockService = lockService;
        _lockService.OnUnlock += (_, _) => OnUnlock();

        InitActivityMonitor(activityMonitor);
        _hotKeyHook = hotKeyHook;
        _taskManagerHook = taskManagerHook;

        // 防止锁屏时系统崩溃、重启等问题导致任务栏被禁用
        // 启动时默认启用一次        
        // TODO 这里需要重新设计，不应该在这里启用任务管理器，因为有可能任务管理器是被其他程序主动禁用的
        _taskManagerHook.EnabledTaskManager();
        _mouseHook = mouseHook;
        _systemKeyHook = systemKeyHook;
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

        _logger.Write("自动锁定已生效");
        _activityMonitor.Init(_appSettings.AutoLockSecond);
        _activityMonitor.OnIdle += (_, _) =>
        {
            _logger.Write("自动锁定 -> 锁定");
            Lock();
        };
        _logger.Write("自动锁定 -> 启动空闲检测");
        _activityMonitor.StartMonitoring();

        _logger.Write("自动锁定 -> 准备监控系统会话状态");
        SystemEvents.SessionSwitch += (_, e) =>
        {
            if (e.Reason == SessionSwitchReason.SessionLock)
            {
                _logger.Write("Windows系统锁定 -> 暂停空闲检测");
                _activityMonitor.StopMonitoring();
            }
            else if (e.Reason == SessionSwitchReason.SessionUnlock)
            {
                _logger.Write("Windows系统解锁 -> 启动空闲检测");
                _activityMonitor.StartMonitoring();
            }
        };
    }

    public void Lock()
    {
        _lockService.Lock();

        _logger.Write("自动锁定 -> 暂停空闲检测");
        _activityMonitor?.StopMonitoring();

        _logger.Write("锁定服务 -> 禁用任务管理器和系统键");
        _taskManagerHook.DisabledTaskManager();

        if (_appSettings.IsHideMouseCursor)
        {
            _logger.Write("锁定服务 -> 隐藏鼠标光标");
            _mouseHook.HideCursor();
        }

        _systemKeyHook.DisableSystemKey();
    }


    private void OnUnlock()
    {
        _logger.Write("自动锁定 -> 启动空闲检测");
        _activityMonitor?.StartMonitoring();

        _logger.Write("锁定服务 -> 恢复任务管理器和系统键");
        _taskManagerHook.EnabledTaskManager();

        if (_appSettings.IsHideMouseCursor)
        {
            _logger.Write("锁定服务 -> 恢复鼠标光标");
            _mouseHook.ResetCursorState();
        }

        _systemKeyHook.Dispose();
    }

    public void Dispose()
    {
        _hotKeyHook.Dispose();
        _activityMonitor?.Dispose();
    }
}