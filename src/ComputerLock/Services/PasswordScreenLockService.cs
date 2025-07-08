using Microsoft.Extensions.DependencyInjection;
using Application = System.Windows.Application;

namespace ComputerLock.Services;

internal class PasswordScreenLockService(IServiceProvider serviceProvider, IStringLocalizer<Lang> lang, ILogger logger, PopupService popupService)
    : ScreenLockBaseService
{
    private bool _showAnimation;
    private WindowLockScreen? _windowLockScreen;
    private readonly List<WindowBlankScreen> _blankScreens = [];

    public override event EventHandler? OnUnlock;
    public override bool Lock(bool showAnimation)
    {
        _showAnimation = showAnimation;
        logger.Write("密码屏幕锁定 -> 准备锁定");
        var primaryScreen = Screen.PrimaryScreen;
        if (primaryScreen == null)
        {
            logger.Write("密码屏幕锁定 -> 没有检测到屏幕");
            return false;
        }

        if (_showAnimation)
        {
            logger.Write("密码屏幕锁定 -> 锁定动画");
            popupService.ShowMessage(lang["Locked"]);
        }

        if (_blankScreens.Count > 0)
        {
            logger.Write("密码屏幕锁定 -> 准备处理副屏");
            _blankScreens.Clear();
        }

        Application.Current.Dispatcher.Invoke(() =>
        {
            _windowLockScreen = serviceProvider.GetRequiredService<WindowLockScreen>();
            _windowLockScreen.OnUnlock += FmLockScreen_OnUnlock;
            _windowLockScreen.Closing += (_, _) =>
            {
                _windowLockScreen.OnUnlock -= FmLockScreen_OnUnlock;
            };
            logger.Write("密码屏幕锁定 -> 激活功能屏幕");
            ShowWindowOnScreen(_windowLockScreen, primaryScreen);

            for (var i = 0; i <= Screen.AllScreens.Length - 1; i++)
            {
                var screen = Screen.AllScreens[i];
                if (screen.Primary)
                {
                    continue;
                }
                logger.Write($"密码屏幕锁定 -> 准备空白屏幕{i}");
                var blankScreen = serviceProvider.GetRequiredService<WindowBlankScreen>();
                blankScreen.OnDeviceInput += BlankScreen_OnDeviceInput;
                logger.Write($"密码屏幕锁定 -> 激活空白屏幕{i}");
                ShowWindowOnScreen(blankScreen, screen);
                _blankScreens.Add(blankScreen);
            }
        });
        return true;
    }

    public override void Unlock()
    {
        logger.Write("密码屏幕锁定 -> 功能屏幕准备解锁");
        _windowLockScreen!.Close();

        foreach (var screen in _blankScreens)
        {
            logger.Write("密码屏幕锁定 -> 释放空白屏幕资源");
            screen.OnDeviceInput -= BlankScreen_OnDeviceInput;
            screen.Unlock();
            screen.Close();
        }

        if (_showAnimation)
        {
            logger.Write("密码屏幕锁定 -> 解锁动画");
            popupService.ShowMessage(lang["UnLocked"]);
        }
    }

    private void FmLockScreen_OnUnlock(object? sender, EventArgs e)
    {
        logger.Write("密码屏幕锁定 -> 通知解锁");
        OnUnlock?.Invoke(this, EventArgs.Empty);
    }
    private void BlankScreen_OnDeviceInput(object? sender, EventArgs e)
    {
        logger.Write("密码屏幕锁定 -> 收到副屏解锁通知");
        _windowLockScreen?.ShowPassword();
    }
}