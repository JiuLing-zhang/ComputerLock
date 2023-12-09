using ComputerLock.Resources;
using JiuLing.AutoUpgrade.Shared;
using JiuLing.AutoUpgrade.Shell;
using JiuLing.CommonLibs.ExtensionMethods;

namespace ComputerLock;
internal class UpdateHelper
{
    private readonly AppSettings _appSettings;
    public UpdateHelper(AppSettings appSettings)
    {
        _appSettings = appSettings;
    }

    public async Task DoAsync(bool isBackgroundCheck)
    {
        var autoUpgradePath = Resource.AutoUpgradePath;
        if (autoUpgradePath.IsEmpty())
        {
            return;
        }
        var theme = _appSettings.AppThemeInt switch
        {
            0 => ThemeEnum.System,
            1 => ThemeEnum.Light,
            2 => ThemeEnum.Dark,
            _ => ThemeEnum.Light
        };
        await AutoUpgradeFactory.Create()
            .UseHttpMode(autoUpgradePath)
            .SetUpgrade(config =>
            {
                config.IsBackgroundCheck = isBackgroundCheck;
                config.Theme = theme;
                config.IsCheckSign = true;
                config.Lang = _appSettings.Lang.ToString();
            })
            .RunAsync();
    }
}