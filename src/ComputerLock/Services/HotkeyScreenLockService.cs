using Microsoft.Extensions.DependencyInjection;
using Application = System.Windows.Application;

namespace ComputerLock.Services;

internal class HotkeyScreenLockService(
    IServiceProvider serviceProvider,
    IStringLocalizer<Lang> lang,
    ILogger logger, PopupService popupService) : ScreenLockBaseService
{
    private bool _showAnimation;
    private readonly List<WindowBlankScreen> _blankScreens = [];

    public override event EventHandler? OnUnlock;
    public override bool Lock(bool showAnimation)
    {
        _showAnimation = showAnimation;
        logger.Info("快捷键屏幕锁定 -> 准备锁定");
        var primaryScreen = Screen.PrimaryScreen;
        if (primaryScreen == null)
        {
            logger.Info("快捷键屏幕锁定 -> 没有检测到屏幕");
            return false;
        }

        if (_showAnimation)
        {
            logger.Info("快捷键屏幕锁定 -> 锁定动画");
            popupService.ShowMessage(lang["Locked"]);
        }

        if (_blankScreens.Count > 0)
        {
            logger.Info("快捷键屏幕锁定 -> 准备初始化屏幕");
            _blankScreens.Clear();
        }

        Application.Current.Dispatcher.Invoke(() =>
        {
            for (var i = 0; i <= Screen.AllScreens.Length - 1; i++)
            {
                var screen = Screen.AllScreens[i];
                logger.Info($"快捷键屏幕锁定 -> 准备屏幕{i}");
                var blankScreen = serviceProvider.GetRequiredService<WindowBlankScreen>();
                ShowWindowOnScreen(blankScreen, screen);
                _blankScreens.Add(blankScreen);
            }
        });
        return true;
    }

    public override void Unlock()
    {
        logger.Info("快捷键屏幕锁定 -> 准备解锁");
        foreach (var screen in _blankScreens)
        {
            logger.Info("快捷键屏幕锁定 -> 释放空白屏幕资源");
            screen.Unlock();
            screen.Close();
        }

        if (_showAnimation)
        {
            logger.Info("快捷键屏幕锁定 -> 解锁动画");
            popupService.ShowMessage(lang["UnLocked"]);
        }
    }
}