using Microsoft.Extensions.DependencyInjection;
using Application = System.Windows.Application;

namespace ComputerLock.Services;

internal class PasswordScreenLockService(IServiceProvider serviceProvider, IStringLocalizer<Lang> lang, ILogger logger)
    : ScreenLockBaseService
{
    private bool _showAnimation;
    private WindowLockScreen? _windowLockScreen;
    private readonly List<WindowBlankScreen> _blankScreens = [];

    public override event EventHandler? OnUnlock;
    public override bool Lock(bool showAnimation)
    {
        _showAnimation = showAnimation;
        logger.Write("锁定服务 -> 准备锁定");
        var primaryScreen = Screen.PrimaryScreen;
        if (primaryScreen == null)
        {
            logger.Write("锁定服务 -> 没有检测到屏幕");
            return false;
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
        return true;
    }

    public override void Unlock()
    {
        // 密码解锁时，没有显式调用解锁方法，而是通过事件触发解锁
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
}