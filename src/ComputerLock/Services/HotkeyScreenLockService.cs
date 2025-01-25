using Microsoft.Extensions.DependencyInjection;
using Application = System.Windows.Application;

namespace ComputerLock.Services;
internal class HotkeyScreenLockService(
    IServiceProvider serviceProvider,
    IStringLocalizer<Lang> lang,
    ILogger logger) : ScreenLockBaseService
{
    private bool _showAnimation;
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
            logger.Write("锁定服务 -> 激活主屏幕");

            for (var i = 0; i <= Screen.AllScreens.Length - 1; i++)
            {
                var screen = Screen.AllScreens[i];
                logger.Write($"锁定服务 -> 准备副屏幕{i}");
                var blankScreen = serviceProvider.GetRequiredService<WindowBlankScreen>();
                ShowWindowOnScreen(blankScreen, screen);
                _blankScreens.Add(blankScreen);
            }
        });
        return true;
    }

    public override void Unlock()
    {
        logger.Write("锁定服务 -> 准备解锁");
        foreach (var screen in _blankScreens)
        {
            logger.Write("锁定服务 -> 释放副屏幕资源");
            screen.Unlock();
            screen.Close();
        }

        if (_showAnimation)
        {
            logger.Write("锁定服务 -> 解锁动画");
            ShowPopup(lang["UnLocked"]);
        }
    }
}