namespace ComputerLock.Components.Settings;
public partial class UnlockSettings
{
    [Inject]
    private AppSettings AppSettings { get; set; } = null!;

    [Inject]
    private AppSettingsProvider AppSettingsProvider { get; set; } = null!;

    [Inject]
    private IStringLocalizer<Lang> Lang { get; set; } = null!;

    [Inject]
    private ISnackbar Snackbar { get; set; } = null!;

    [Inject]
    private IDialogService Dialog { get; set; } = null!;

    [Inject]
    private HotkeyHook HotkeyHook { get; set; } = null!;

    [Inject]
    private ILogger Logger { get; set; } = null!;


    private bool _keyboardDownChecked;
    private bool _mouseDownChecked;

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        _keyboardDownChecked = (AppSettings.PasswordBoxActiveMethod & PasswordBoxActiveMethodEnum.KeyboardDown) == PasswordBoxActiveMethodEnum.KeyboardDown;
        _mouseDownChecked = (AppSettings.PasswordBoxActiveMethod & PasswordBoxActiveMethodEnum.MouseDown) == PasswordBoxActiveMethodEnum.MouseDown;
    }
    private void SelectUnlockMethod(ScreenUnlockMethods method)
    {
        if (AppSettings.ScreenUnlockMethod != method)
        {
            AppSettings.ScreenUnlockMethod = method;
            SaveSettings();
        }
    }

    private void SaveSettings()
    {
        AppSettingsProvider.SaveSettings(AppSettings);
    }

    private async Task PasswordEdit()
    {
        IDialogReference dialog;
        var options = new DialogOptions { NoHeader = true, CloseOnEscapeKey = false, BackdropClick = false, BackgroundClass = "dialog-backdrop-filter" };

        if (AppSettings.Password.IsEmpty())
        {
            dialog = await Dialog.ShowAsync<SetPassword>("", options);
        }
        else
        {
            dialog = await Dialog.ShowAsync<ResetPassword>("", options);
        }
        var result = await dialog.Result;
        if (result == null || result.Canceled)
        {
            return;
        }
        if (result.Data == null)
        {
            return;
        }

        AppSettings.Password = result.Data.ToString()!;
        SaveSettings();
    }

    private void KeyboardDownChecked()
    {
        if (!_keyboardDownChecked)
        {
            if (!_mouseDownChecked)
            {
                _keyboardDownChecked = true;
                Snackbar.Add(Lang["ActiveMethodEmpty"], Severity.Error);
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
                Snackbar.Add(Lang["ActiveMethodEmpty"], Severity.Error);
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

    private void PwdBoxLocationChanged(ScreenLocationEnum location)
    {
        AppSettings.PasswordInputLocation = location;
        SaveSettings();
    }

    private Task SetUnlockHotkey(string hotkey)
    {
        AppSettings.UnlockHotkeyString = hotkey;
        SaveSettings();
        RegisterUnlockHotkey();
        return Task.CompletedTask;
    }
    private Task ClearUnlockHotkey()
    {
        AppSettings.UnlockHotkeyString = "";
        SaveSettings();
        UnregisterUnlockHotkey();
        return Task.CompletedTask;
    }

    public void RegisterUnlockHotkey()
    {
        try
        {
            if (AppSettings.UnlockHotkey != null)
            {
                Logger.Write("注册解锁热键");
                HotkeyHook.Register((int)HotkeyType.Unlock, AppSettings.UnlockHotkey);
            }
        }
        catch (Exception ex)
        {
            Logger.Write($"绑定解锁热键失败：{ex.Message}。{ex.StackTrace}");
            Snackbar.Add($"{Lang["ExRegistFailed"]}{ex.Message}", Severity.Error);
        }
    }
    public void UnregisterUnlockHotkey()
    {
        try
        {
            Logger.Write("释放解锁热键");
            HotkeyHook.Unregister((int)HotkeyType.Unlock);
        }
        catch (Exception ex)
        {
            Logger.Write($"释放解锁热键失败：{ex.Message}。{ex.StackTrace}");
            //MessageBoxUtils.ShowError($"取消快捷键失败：{ex.Message}");
        }
    }
}
