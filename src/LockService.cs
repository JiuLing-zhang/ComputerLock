using ComputerLock.Hooks;
using ComputerLock.Resources;
using JiuLing.CommonLibs.Log;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using System.Windows;
using System.Windows.Threading;

namespace ComputerLock;
internal class LockService
{
    private bool _isLocked = false;
    private readonly IServiceProvider _serviceProvider;
    private readonly SystemKeyHook _systemKeyHook = new();
    private WindowLockScreen _windowLockScreen;
    private readonly List<WindowBlankScreen> _blankScreens;
    private WindowPopup? _popup;
    private readonly IStringLocalizer<Lang> _lang;
    private readonly AppSettings _appSettings;
    private readonly ILogger _logger;
    public event EventHandler OnLock;
    public event EventHandler OnUnlock;
    public LockService(IServiceProvider serviceProvider, IStringLocalizer<Lang> lang, AppSettings appSettings, ILogger logger)
    {
        _serviceProvider = serviceProvider;
        _blankScreens = new List<WindowBlankScreen>();
        _lang = lang;
        _appSettings = appSettings;
        _logger = logger;

        // 防止锁屏时系统崩溃、重启等问题导致任务栏被禁用
        // 启动时默认启用一次
        TaskManagerHook.EnabledTaskManager();
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
        TaskManagerHook.DisabledTaskManager();
        _systemKeyHook.DisableSystemKey();
        if (_blankScreens.Any())
        {
            _blankScreens.Clear();
        }

        _logger.Write("锁定服务 -> 准备主屏幕");
        _windowLockScreen = _serviceProvider.GetRequiredService<WindowLockScreen>();
        _windowLockScreen.Left = primaryScreen.WorkingArea.Left;
        _windowLockScreen.Top = primaryScreen.WorkingArea.Top;
        _windowLockScreen.OnUnlock += _fmLockScreen_OnUnlock;
        _windowLockScreen.Closing += (_, __) =>
        {
            _windowLockScreen.OnUnlock -= _fmLockScreen_OnUnlock;
        };
        _windowLockScreen.Show();
        _windowLockScreen.Activate();
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
            blankScreen.WindowStartupLocation = WindowStartupLocation.Manual;
            blankScreen.Left = screen.WorkingArea.Left;
            blankScreen.Top = screen.WorkingArea.Top;
            blankScreen.OnDeviceInput += BlankScreen_OnDeviceInput;
            blankScreen.Show();
            blankScreen.Activate();
            _logger.Write("锁定服务 -> 激活副屏幕");
            _blankScreens.Add(blankScreen);
        }

        OnLock?.Invoke(this, EventArgs.Empty);
    }

    private void _fmLockScreen_OnUnlock(object? sender, EventArgs e)
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
        TaskManagerHook.EnabledTaskManager();
        _systemKeyHook.Dispose();
        _isLocked = false;
        if (_appSettings.LockAnimation)
        {
            _logger.Write("锁定服务 -> 解锁动画");
            ShowPopup(_lang["UnLocked"]);
        }
        _logger.Write("锁定服务 -> 通知解锁");
        OnUnlock?.Invoke(this, EventArgs.Empty);
    }
    private void BlankScreen_OnDeviceInput(object? sender, EventArgs e)
    {
        _logger.Write("锁定服务 -> 收到副屏解锁通知");
        _windowLockScreen.ShowPassword();
    }

    private void ShowPopup(string message)
    {
        _popup = new WindowPopup(message);
        double primaryScreenWidth = SystemParameters.PrimaryScreenWidth;
        double primaryScreenHeight = SystemParameters.PrimaryScreenHeight;
        _popup.Left = (primaryScreenWidth - _popup.Width) / 2;
        _popup.Top = (primaryScreenHeight - _popup.Height) / 2;
        _popup.Show();

        var timer = new DispatcherTimer()
        {
            Interval = TimeSpan.FromMilliseconds(1100),
        };
        timer.Tick += (_, __) =>
        {
            timer.Stop();
            _popup.CloseWindow();
        };
        timer.Start();
    }
}