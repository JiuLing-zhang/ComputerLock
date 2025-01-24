using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using System.Windows.Threading;
using Application = System.Windows.Application;

namespace ComputerLock.Services;

/// <summary>
/// 负责屏幕窗口的锁定和解锁
/// </summary>
internal class ScreenLockService(
    IServiceProvider serviceProvider,
    IStringLocalizer<Lang> lang,
    ILogger logger)
{
    private bool _showAnimation;
    private WindowLockScreen? _windowLockScreen;
    private readonly List<WindowBlankScreen> _blankScreens = [];
    public event EventHandler? OnUnlock;

    public void Lock(bool showAnimation)
    {
        _showAnimation = showAnimation;
        logger.Write("锁定服务 -> 准备锁定");
        var primaryScreen = Screen.PrimaryScreen;
        if (primaryScreen == null)
        {
            logger.Write("锁定服务 -> 没有检测到屏幕");
            throw new Exception("没有检测到屏幕 no screen");
        }

        if (_showAnimation)
        {
            logger.Write("锁定服务 -> 锁定动画");
            ShowPopup(lang["Locked"]);
        }

        if (_blankScreens.Count > 0)
        {
            _blankScreens.Clear();
        }

        logger.Write("锁定服务 -> 准备主屏幕");
        Application.Current.Dispatcher.Invoke(() =>
        {
            _windowLockScreen = serviceProvider.GetRequiredService<WindowLockScreen>();
            _windowLockScreen.OnUnlock += FmLockScreen_OnUnlock;
            _windowLockScreen.Closing += (_, _) =>
            {
                _windowLockScreen.OnUnlock -= FmLockScreen_OnUnlock;
            };
            ShowWindowOnScreen(_windowLockScreen, primaryScreen);

            logger.Write("锁定服务 -> 激活主屏幕");

            for (var i = 0; i <= Screen.AllScreens.Length - 1; i++)
            {
                var screen = Screen.AllScreens[i];
                if (screen.Primary)
                {
                    continue;
                }
                logger.Write($"锁定服务 -> 准备副屏幕{i}");
                var blankScreen = serviceProvider.GetRequiredService<WindowBlankScreen>();
                blankScreen.OnDeviceInput += BlankScreen_OnDeviceInput;
                ShowWindowOnScreen(blankScreen, screen);
                _blankScreens.Add(blankScreen);
            }
        });
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
        window.Loaded += (_, _) =>
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
        logger.Write("锁定服务 -> 准备解锁");
        foreach (var screen in _blankScreens)
        {
            logger.Write("锁定服务 -> 释放副屏幕资源");
            screen.OnDeviceInput -= BlankScreen_OnDeviceInput;
            screen.Unlock();
            screen.Close();
        }

        if (_showAnimation)
        {
            logger.Write("锁定服务 -> 解锁动画");
            ShowPopup(lang["UnLocked"]);
        }

        logger.Write("锁定服务 -> 通知解锁");
        OnUnlock?.Invoke(this, EventArgs.Empty);
    }
    private void BlankScreen_OnDeviceInput(object? sender, EventArgs e)
    {
        logger.Write("锁定服务 -> 收到副屏解锁通知");
        _windowLockScreen?.ShowPassword();
    }

    private void ShowPopup(string message)
    {
        Application.Current.Dispatcher.Invoke(() =>
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
            timer.Tick += (_, _) =>
            {
                timer.Stop();
                popup.CloseWindow();
            };
            timer.Start();
        });
    }
}