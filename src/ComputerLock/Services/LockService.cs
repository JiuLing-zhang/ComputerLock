using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using System.Windows.Threading;

namespace ComputerLock.Services;
internal class LockService
{
    private bool _isLocked = false;
    private readonly IServiceProvider _serviceProvider;
    private readonly SystemKeyHook _systemKeyHook;
    private WindowLockScreen? _windowLockScreen;
    private readonly List<WindowBlankScreen> _blankScreens = [];
    private readonly IStringLocalizer<Lang> _lang;
    private readonly AppSettings _appSettings;
    private readonly ILogger _logger;
    private readonly TaskManagerHook _taskManagerHook;
    private readonly MouseHook _mouseHook;
    public event EventHandler? OnLock;
    public event EventHandler? OnUnlock;
    public LockService(IServiceProvider serviceProvider, SystemKeyHook systemKeyHook, IStringLocalizer<Lang> lang, AppSettings appSettings, ILogger logger, TaskManagerHook taskManagerHook, MouseHook mouseHook)
    {
        _serviceProvider = serviceProvider;
        _systemKeyHook = systemKeyHook;
        _lang = lang;
        _appSettings = appSettings;
        _logger = logger;
        _taskManagerHook = taskManagerHook;
        _mouseHook = mouseHook;

        // 防止锁屏时系统崩溃、重启等问题导致任务栏被禁用
        // 启动时默认启用一次
        _taskManagerHook.EnabledTaskManager();
    }

    public void Lock()
    {
        _logger.Write("锁定服务 -> 准备锁定");
        if (_isLocked)
        {
            return;
        }
        _logger.Write("锁定服务 -> 允许锁定");
        var primaryScreen = Screen.PrimaryScreen;
        if (primaryScreen == null)
        {
            _logger.Write("锁定服务 -> 没有检测到屏幕");
            throw new Exception("没有检测到屏幕 no screen");
        }

        _isLocked = true;
        if (_appSettings.LockAnimation)
        {
            _logger.Write("锁定服务 -> 锁定动画");
            ShowPopup(_lang["Locked"]);
        }

        _logger.Write("锁定服务 -> 禁用任务管理器和系统键");
        _taskManagerHook.DisabledTaskManager();
        _systemKeyHook.DisableSystemKey();
        if (_blankScreens.Count > 0)
        {
            _blankScreens.Clear();
        }

        _logger.Write("锁定服务 -> 准备主屏幕");
        _windowLockScreen = _serviceProvider.GetRequiredService<WindowLockScreen>();
        _windowLockScreen.OnUnlock += FmLockScreen_OnUnlock;
        _windowLockScreen.Closing += (_, _) =>
        {
            _windowLockScreen.OnUnlock -= FmLockScreen_OnUnlock;
        };
        ShowWindowOnScreen(_windowLockScreen, primaryScreen);

        _logger.Write("锁定服务 -> 激活主屏幕");
        for (var i = 0; i <= Screen.AllScreens.Length - 1; i++)
        {
            var screen = Screen.AllScreens[i];
            if (screen.Primary)
            {
                continue;
            }
            _logger.Write($"锁定服务 -> 准备副屏幕{i}");

            var blankScreen = _serviceProvider.GetRequiredService<WindowBlankScreen>();
            blankScreen.OnDeviceInput += BlankScreen_OnDeviceInput;
            ShowWindowOnScreen(blankScreen, screen);

            _logger.Write("锁定服务 -> 激活副屏幕");
            _blankScreens.Add(blankScreen);
        }

        if (_appSettings.IsHideMouseCursor)
        {
            _logger.Write("锁定服务 -> 隐藏鼠标光标");
            _mouseHook.HideCursor();
        }
        OnLock?.Invoke(this, EventArgs.Empty);
    }

    private void ShowWindowOnScreen(Window window, Screen screen)
    {
        // 获取包括任务栏的完整屏幕区域
        var bounds = screen.Bounds;

        // 设置窗口初始位置和大小
        window.WindowStartupLocation = WindowStartupLocation.Manual;
        window.Left = bounds.Left;
        window.Top = bounds.Top;
        window.Width = bounds.Width;
        window.Height = bounds.Height;

        // 在窗口加载后，根据屏幕的 DPI 重新调整位置和大小
        // 必须先显示窗口，然后才能获取 DPI，所以窗口大小和位置需要二次调整
        window.Loaded += (sender, e) =>
        {
            var dpiFactor = GetDpiFactor(window);
            window.Left = bounds.Left / dpiFactor.X;
            window.Top = bounds.Top / dpiFactor.Y;
            window.Width = bounds.Width / dpiFactor.X;
            window.Height = bounds.Height / dpiFactor.Y;
        };

        window.WindowStyle = WindowStyle.None;
        window.ResizeMode = System.Windows.ResizeMode.NoResize;

        window.Show();
        window.Activate();
    }

    private (double X, double Y) GetDpiFactor(Window window)
    {
        var source = PresentationSource.FromVisual(window);
        if (source?.CompositionTarget != null)
        {
            var transform = source.CompositionTarget.TransformToDevice;
            return (transform.M11, transform.M22);
        }
        return (1.0, 1.0); // 默认比例
    }
    private void FmLockScreen_OnUnlock(object? sender, EventArgs e)
    {
        _logger.Write("锁定服务 -> 准备解锁");
        foreach (var screen in _blankScreens)
        {
            _logger.Write("锁定服务 -> 释放副屏幕资源");
            screen.OnDeviceInput -= BlankScreen_OnDeviceInput;
            screen.Unlock();
            screen.Close();
        }
        _logger.Write("锁定服务 -> 恢复任务管理器和系统键");
        _taskManagerHook.EnabledTaskManager();
        _systemKeyHook.Dispose();
        _isLocked = false;
        if (_appSettings.LockAnimation)
        {
            _logger.Write("锁定服务 -> 解锁动画");
            ShowPopup(_lang["UnLocked"]);
        }

        if (_appSettings.IsHideMouseCursor)
        {
            _logger.Write("锁定服务 -> 恢复鼠标光标");
            _mouseHook.ResetCursorState();
        }

        _logger.Write("锁定服务 -> 通知解锁");
        OnUnlock?.Invoke(this, EventArgs.Empty);
    }
    private void BlankScreen_OnDeviceInput(object? sender, EventArgs e)
    {
        _logger.Write("锁定服务 -> 收到副屏解锁通知");
        _windowLockScreen?.ShowPassword();
    }

    private void ShowPopup(string message)
    {
        var popup = new WindowPopup(message);
        double primaryScreenWidth = SystemParameters.PrimaryScreenWidth;
        double primaryScreenHeight = SystemParameters.PrimaryScreenHeight;
        popup.Left = (primaryScreenWidth - popup.Width) / 2;
        popup.Top = (primaryScreenHeight - popup.Height) / 2;
        popup.Show();

        var timer = new DispatcherTimer()
        {
            Interval = TimeSpan.FromMilliseconds(1100),
        };
        timer.Tick += (_, __) =>
        {
            timer.Stop();
            popup.CloseWindow();
        };
        timer.Start();
    }
}