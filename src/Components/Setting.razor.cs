using System.Diagnostics;
using System.IO;

namespace ComputerLock.Components;
public partial class Setting
{
    private bool _isOpen;
    private bool _isAutostart;
    private string _version = "";
    private readonly string _logPath = Path.Combine(Environment.CurrentDirectory, "log");
    private bool _logLoadingOk = false;
    private double _logFilesSize = 0;

    [Inject]
    private AppSettings AppSettings { get; set; } = default!;
    [Inject]
    private AppSettingWriter AppSettingWriter { get; set; } = default!;
    [Inject]
    private IStringLocalizer<Lang> Lang { get; set; } = default!;
    [Inject]
    private AutostartHook AutostartHook { get; set; } = default!;
    [Inject]
    private ISnackbar Snackbar { get; set; } = default!;
    [Inject]
    private UpdateHelper UpdateHelper { get; set; } = default!;
    [Inject]
    private IWindowTitleBar WindowTitleBar { get; set; } = default!;
    [Inject]
    private IDialogService DialogService { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        _isAutostart = AutostartHook.IsAutostart();
        _version = $"v{AppBase.Version[..AppBase.Version.LastIndexOf('.')]}";
    }

    public async Task OpenAsync()
    {
        _isOpen = true;
        await CalcLogSizeAsync();
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
        catch (Exception)
        {
        }
        finally
        {
            _logLoadingOk = true;
            await InvokeAsync(StateHasChanged);
        }
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
        }
        catch (Exception)
        {
        }
        finally
        {
            Snackbar.Add(Lang["RemoveLogsOk"], Severity.Success);
            await CalcLogSizeAsync();
        }
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
        AppSettingWriter.Save(AppSettings);
    }

    private async Task CheckUpdateAsync()
    {
        await UpdateHelper.DoAsync(false);
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
            config.Onclick = snackbar =>
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

        AppSettingWriter.Save(new AppSettings());
        Restart();
    }
}