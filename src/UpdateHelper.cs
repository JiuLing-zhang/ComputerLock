﻿using ComputerLock.Resources;
using JiuLing.AutoUpgrade.Shared;
using JiuLing.AutoUpgrade.Shell;
using JiuLing.CommonLibs.ExtensionMethods;
using System.IO;

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
        var iconPath = @"wwwroot\icon.ico";
        if (!File.Exists(iconPath))
        {
            iconPath = "";
        }
        await AutoUpgradeFactory.Create()
            .UseHttpMode(autoUpgradePath)
            .SetUpgrade(config =>
            {
                config.IsBackgroundCheck = isBackgroundCheck;
                config.Theme = theme;
                config.IsCheckSign = true;
                config.Lang = _appSettings.Lang.ToString();
                config.IconPath = iconPath;
            })
            .RunAsync();
    }
}