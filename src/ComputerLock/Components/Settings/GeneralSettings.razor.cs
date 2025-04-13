using System.Diagnostics;
using System.IO;
using ComputerLock.Interfaces;

namespace ComputerLock.Components.Settings;

public partial class GeneralSettings
{
    private bool _isAutostart;
    private readonly string _logPath = Path.Combine(Environment.CurrentDirectory, "log");
    private bool _logLoadingOk;
    private double _logFilesSize;

    [Inject]
    private AppSettings AppSettings { get; set; } = null!;
    [Inject]
    private AppSettingsProvider AppSettingsProvider { get; set; } = null!;
    [Inject]
    private IStringLocalizer<Lang> Lang { get; set; } = null!;
    [Inject]
    private AutostartHook AutostartHook { get; set; } = null!;
    [Inject]
    private ISnackbar Snackbar { get; set; } = null!;
    [Inject]
    private IWindowTitleBar WindowTitleBar { get; set; } = null!;
    [Inject]
    private IDialogService DialogService { get; set; } = null!;
    [Inject]
    private ThemeSwitchService ThemeSwitchService { get; set; } = null!;

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        _isAutostart = AutostartHook.IsAutostart();
        await CalcLogSizeAsync();
    }

    private void AutostartChange(bool isChecked)
    {
        if (isChecked)
        {
            AutostartHook.EnabledAutostart();
        }
        else
        {
            AutostartHook.DisabledAutostart();
        }
        _isAutostart = AutostartHook.IsAutostart();
    }
    private void SaveSettings()
    {
        AppSettingsProvider.SaveSettings(AppSettings);
    }

    private void ThemeChanged(ThemeEnum theme)
    {
        AppSettings.AppThemeInt = theme;
        SaveSettings();
        ThemeSwitchService.SetDarkMode(theme);
    }

    private Task LangValueChanged(string lang)
    {
        LangEnum langEnum = (LangEnum)Enum.Parse(typeof(LangEnum), lang);
        AppSettings.Lang = langEnum;
        SaveSettings();
        RestartTips();
        return Task.CompletedTask;
    }

    private void RestartTips()
    {
        Snackbar.Configuration.NewestOnTop = true;
        Snackbar.Configuration.VisibleStateDuration = int.MaxValue;
        Snackbar.Configuration.ShowCloseIcon = true;
        Snackbar.Configuration.SnackbarVariant = Variant.Text;
        Snackbar.Add("重启后生效 Take effect after restart", Severity.Normal, config =>
        {
            config.Action = "重启 Restart";
            config.HideIcon = true;
            config.ActionColor = MudBlazor.Color.Warning;
            config.ActionVariant = Variant.Outlined;
            config.OnClick = _ =>
            {
                Restart();
                return Task.CompletedTask;
            };
        });
    }

    private void Restart()
    {
        WindowTitleBar.Restart();
    }

    private void OpenLogPath()
    {
        Process.Start("explorer.exe", _logPath);
    }

    private async Task ClearLogAsync()
    {
        bool? result = await DialogService.ShowMessageBox(
            Lang["Warning"],
            Lang["RemoveLogsConfirm"],
            yesText: Lang["Delete"],
            cancelText: Lang["Cancel"]);
        if (result != true)
        {
            return;
        }

        try
        {
            if (!Directory.Exists(_logPath))
            {
                return;
            }
            var files = Directory.GetFiles(_logPath);
            foreach (var file in files)
            {
                File.Delete(file);
            }

            Snackbar.Add(Lang["RemoveLogsOk"], Severity.Success);
            await CalcLogSizeAsync();
        }
        catch (Exception ex)
        {
            Snackbar.Add($"日志文件删除失败：{ex.Message}", Severity.Error);
        }
    }

    private async Task CalcLogSizeAsync()
    {
        try
        {
            _logFilesSize = 0;
            _logLoadingOk = false;
            await InvokeAsync(StateHasChanged);

            if (!Directory.Exists(_logPath))
            {
                return;
            }
            var files = Directory.GetFiles(_logPath);
            foreach (var file in files)
            {
                var fi = new FileInfo(file);
                _logFilesSize += fi.Length;
            }
        }
        catch (Exception ex)
        {
            Snackbar.Add($"日志文件计算失败：{ex.Message}", Severity.Error);
        }
        finally
        {
            _logLoadingOk = true;
            await InvokeAsync(StateHasChanged);
        }
    }

    private async Task ResetSettingsAsync()
    {
        bool? result = await DialogService.ShowMessageBox(
            Lang["Warning"],
            Lang["ResetSettingsMessage"],
            yesText: Lang["Save"],
            cancelText: Lang["Cancel"]);
        if (result != true)
        {
            return;
        }

        AppSettingsProvider.RemoveSettings();
        Restart();
    }
}