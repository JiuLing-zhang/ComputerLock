using System.IO;
using JiuLing.AutoUpgrade.Shared;
using JiuLing.AutoUpgrade.Shell;
using JiuLing.AutoUpgrade.Shell.Enums;

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
        var theme = appSettings.AppThemeInt switch
        {
            0 => ThemeEnum.System,
            1 => ThemeEnum.Light,
            2 => ThemeEnum.Dark,
            _ => ThemeEnum.Light
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