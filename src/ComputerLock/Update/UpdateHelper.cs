using JiuLing.AutoUpgrade.Shell;
using JiuLing.AutoUpgrade.Shell.Enums;
using System.IO;
using ThemeEnum = JiuLing.AutoUpgrade.Shared.ThemeEnum;

namespace ComputerLock.Update;

internal class UpdateHelper(AppSettings appSettings)
{
    public async Task DoAsync(bool isBackgroundCheck)
    {
        var autoUpgradePath = Resource.AutoUpgradePath;
        if (autoUpgradePath.IsEmpty())
        {
            return;
        }
        var theme = appSettings.AppTheme switch
        {
            Enums.ThemeEnum.System => ThemeEnum.System,
            Enums.ThemeEnum.Light => ThemeEnum.Light,
            Enums.ThemeEnum.Dark => ThemeEnum.Dark,
            _ => ThemeEnum.System
        };
        var iconPath = @"wwwroot\icon.ico";
        if (!File.Exists(iconPath))
        {
            iconPath = "";
        }

        await UpgradeFactory.CreateHttpApp(autoUpgradePath)
            .SetUpgrade(build =>
        {
            build.WithBackgroundCheck(isBackgroundCheck)
            .WithTheme(theme)
                .WithSignCheck(true)
                .WithLang(appSettings.Lang.ToString())
                .WithIcon(iconPath)
                .WithVersionFormat(VersionFormatEnum.MajorMinorBuild);
        }).RunAsync();
    }
}