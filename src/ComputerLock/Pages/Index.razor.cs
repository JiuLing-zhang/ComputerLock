using ComputerLock.Interfaces;
using ComputerLock.Update;

namespace ComputerLock.Pages;
public partial class Index
{
    [Inject]
    private AppSettings AppSettings { get; set; } = default!;

    [Inject]
    private AppSettingsProvider AppSettingsProvider { get; set; } = default!;

    [Inject]
    private IStringLocalizer<Lang> Lang { get; set; } = default!;

    [Inject]
    private ISnackbar Snackbar { get; set; } = default!;

    [Inject]
    private IDialogService Dialog { get; set; } = default!;

    [Inject]
    private UpdateHelper UpdateHelper { get; set; } = default!;

    [Inject]
    private IGlobalLockService GlobalLockService { get; set; } = default!;

    [Inject]
    private HotkeyHook HotkeyHook { get; set; } = default!;

    [Inject]
    private IWindowTitleBar WindowTitleBar { get; set; } = default!;

    [Inject]
    private ILogger Logger { get; set; } = default!;


    private bool _keyboardDownChecked;
    private bool _mouseDownChecked;
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        _keyboardDownChecked = (AppSettings.PasswordBoxActiveMethod & PasswordBoxActiveMethodEnum.KeyboardDown) == PasswordBoxActiveMethodEnum.KeyboardDown;
        _mouseDownChecked = (AppSettings.PasswordBoxActiveMethod & PasswordBoxActiveMethodEnum.MouseDown) == PasswordBoxActiveMethodEnum.MouseDown;

        if (AppSettings.ShortcutKeyForLock.IsNotTrimEmpty())
        {
            RegisterHotkey();
        }

        HotkeyHook.HotkeyPressed += () =>
        {
            if (GlobalLockService.IsLocked)
            {
                Logger.Write("快捷键解锁");
                GlobalLockService.Unlock();
            }
            else
            {
                Logger.Write("快捷键锁定");
                GlobalLockService.Lock();
            }
        };

        if (AppSettings.IsAutoCheckUpdate)
        {
            await UpdateHelper.DoAsync(true);
        }
    }
    private void KeyboardDownChecked()
    {
        if (!_keyboardDownChecked)
        {
            if (!_mouseDownChecked)
            {
                _keyboardDownChecked = true;
                Snackbar.Add(Lang["ActiveMethodEmpty"], Severity.Warning);
                return;
            }
        }
        SavePasswordBoxActiveMethod();
    }
    private void MouseDownChecked()
    {
        if (!_mouseDownChecked)
        {
            if (!_keyboardDownChecked)
            {
                _mouseDownChecked = true;
                Snackbar.Add(Lang["ActiveMethodEmpty"], Severity.Warning);
                return;
            }
        }
        SavePasswordBoxActiveMethod();
    }

    private void SavePasswordBoxActiveMethod()
    {
        AppSettings.PasswordBoxActiveMethod = 0;
        if (_keyboardDownChecked)
        {
            AppSettings.PasswordBoxActiveMethod |= PasswordBoxActiveMethodEnum.KeyboardDown;
        }
        if (_mouseDownChecked)
        {
            AppSettings.PasswordBoxActiveMethod |= PasswordBoxActiveMethodEnum.MouseDown;
        }
        SaveSettings();
    }

    private void AutoLockChanged(int autoLockMinute)
    {
        AppSettings.AutoLockSecond = autoLockMinute * 60;
        SaveSettings();
        RestartTips();
    }

    private void PwdBoxLocationChanged(ScreenLocationEnum location)
    {
        AppSettings.PasswordInputLocation = location;
        SaveSettings();
        RestartTips();
    }
    private void RestartTips()
    {
        Snackbar.Configuration.NewestOnTop = true;
        Snackbar.Configuration.VisibleStateDuration = int.MaxValue;
        Snackbar.Configuration.ShowCloseIcon = true;
        Snackbar.Configuration.SnackbarVariant = Variant.Text;
        Snackbar.Add(Lang["Restarting"], Severity.Normal, config =>
        {
            config.Action = Lang["Restart"];
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

    private void SaveSettings()
    {
        AppSettingsProvider.SaveSettings(AppSettings);
    }
    private async Task SetShortcutKey()
    {
        var noHeader = new DialogOptions()
        {
            NoHeader = true,
            BackgroundClass = "dialog-blurry",
            CloseOnEscapeKey = false,
        };
        var dialog = await Dialog.ShowAsync<ShortcutKeySetting>("", noHeader);
        var result = await dialog.Result;
        if (result!.Canceled)
        {
            return;
        }

        AppSettings.ShortcutKeyForLock = result.Data!.ToString()!;
        SaveSettings();
        RegisterHotkey();
    }
    private Task ClearShortcutKey()
    {
        AppSettings.ShortcutKeyForLock = "";
        SaveSettings();
        UnregisterHotkey();
        return Task.CompletedTask;
    }

    public void RegisterHotkey()
    {
        try
        {
            if (AppSettings.LockHotkey != null)
            {
                Logger.Write("注册锁屏热键");
                HotkeyHook.Register(AppSettings.LockHotkey);
            }
        }
        catch (Exception ex)
        {
            Logger.Write($"绑定锁屏热键失败：{ex.Message}。{ex.StackTrace}");
            Snackbar.Add($"{Lang["ExRegistFailed"]}{ex.Message}", Severity.Error);
        }
    }

    public void UnregisterHotkey()
    {
        try
        {
            Logger.Write("释放锁屏热键");
            HotkeyHook.Unregister();
        }
        catch (Exception ex)
        {
            Logger.Write($"释放锁屏热键失败：{ex.Message}。{ex.StackTrace}");
            //MessageBoxUtils.ShowError($"取消快捷键失败：{ex.Message}");
        }
    }

    private async Task ResetPassword()
    {
        var noHeader = new DialogOptions()
        {
            BackgroundClass = "dialog-blurry",
            CloseOnEscapeKey = false,
            CloseButton = true
        };
        var dialog = await Dialog.ShowAsync<ResetPassword>(Lang["ResetPassword"], noHeader);
        var result = await dialog.Result;
        if (result.Canceled)
        {
            return;
        }
        if (result.Data == null)
        {
            return;
        }

        AppSettings.Password = result.Data.ToString();
        SaveSettings();
    }

    private async Task OnPasswordSetFinishedAsync(string password)
    {
        AppSettings.Password = password;
        AppSettings.IsPasswordChanged = true;
        SaveSettings();
        await InvokeAsync(StateHasChanged);
    }
}