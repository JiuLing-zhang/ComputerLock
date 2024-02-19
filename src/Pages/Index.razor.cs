namespace ComputerLock.Pages;
public partial class Index
{
    [Inject]
    private AppSettings AppSettings { get; set; } = default!;

    [Inject]
    private AppSettingWriter AppSettingWriter { get; set; } = default!;

    [Inject]
    private IStringLocalizer<Lang> Lang { get; set; } = default!;

    [Inject]
    private ISnackbar Snackbar { get; set; } = default!;

    [Inject]
    private IDialogService Dialog { get; set; } = default!;

    [Inject]
    private UpdateHelper UpdateHelper { get; set; } = default!;

    [Inject]
    private ILocker Locker { get; set; } = default!;

    [Inject]
    private KeyboardHook KeyboardHook { get; set; } = default!;

    [Inject]
    private IWindowTitleBar WindowTitleBar { get; set; } = default!;

    [Inject]
    private ILogger Logger { get; set; } = default!;

    private bool _keyboardDownChecked;
    private bool _mouseDownChecked;
    private string _shortcutKeyText = "未设置";
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        _keyboardDownChecked = (AppSettings.PasswordBoxActiveMethod & PasswordBoxActiveMethodEnum.KeyboardDown) == PasswordBoxActiveMethodEnum.KeyboardDown;
        _mouseDownChecked = (AppSettings.PasswordBoxActiveMethod & PasswordBoxActiveMethodEnum.MouseDown) == PasswordBoxActiveMethodEnum.MouseDown;
        UpdateShortcutKeyForLock();

        KeyboardHook.KeyPressed += (_, _) =>
        {
            Logger.Write("快捷键解锁");
            Locker.Lock();
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

    private void AutoLockChanged(int autoLockSecond)
    {
        AppSettings.AutoLockSecond = autoLockSecond;
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
            config.Onclick = _ =>
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
        AppSettingWriter.Save(AppSettings);
    }
    private async Task SetShortcutKey()
    {
        var noHeader = new DialogOptions()
        {
            NoHeader = true,
            ClassBackground = "dialog-blurry",
            CloseOnEscapeKey = false,
        };
        var dialog = await Dialog.ShowAsync<ShortcutKeySetting>("", noHeader);
        var result = await dialog.Result;
        if (result.Canceled)
        {
            return;
        }

        if (result.Data is not ShortcutKeyModel shortcutKeyModel)
        {
            return;
        }

        AppSettings.ShortcutKeyForLock = shortcutKeyModel.ShortcutKey;
        AppSettings.ShortcutKeyDisplayForLock = shortcutKeyModel.ShortcutKeyDisplay;
        SaveSettings();
        UpdateShortcutKeyForLock();
    }
    private Task ClearShortcutKey()
    {
        AppSettings.ShortcutKeyForLock = "";
        AppSettings.ShortcutKeyDisplayForLock = "";
        SaveSettings();
        UpdateShortcutKeyForLock();
        return Task.CompletedTask;
    }

    public void UpdateShortcutKeyForLock()
    {
        if (AppSettings.ShortcutKeyForLock.IsEmpty())
        {
            try
            {
                Logger.Write("释放锁屏热键");
                KeyboardHook.UnregisterHotKey();
            }
            catch (Exception ex)
            {
                Logger.Write($"释放锁屏热键失败：{ex.Message}。{ex.StackTrace}");
                //MessageBoxUtils.ShowError($"取消快捷键失败：{ex.Message}");
            }
            _shortcutKeyText = Lang["Invalid"];
        }
        else
        {
            try
            {
                ModifierKeys keys = 0;
                if (AppSettings.ShortcutKeyForLock.IndexOf("Ctrl") >= 0)
                {
                    keys |= ModifierKeys.Control;
                }
                if (AppSettings.ShortcutKeyForLock.IndexOf("Shift") >= 0)
                {
                    keys |= ModifierKeys.Shift;
                }
                if (AppSettings.ShortcutKeyForLock.IndexOf("Alt") >= 0)
                {
                    keys |= ModifierKeys.Alt;
                }

                var result = RegexUtils.GetFirst(AppSettings.ShortcutKeyForLock, @"\d+");
                if (result.success == false)
                {
                    throw new Exception(Lang["ShortcutKeyConfigError"]);
                }
                Logger.Write("注册锁屏热键");
                Keys key = (Keys)Convert.ToInt32(result.result);
                KeyboardHook.RegisterHotKey(keys, key);

                _shortcutKeyText = AppSettings.ShortcutKeyDisplayForLock;
            }
            catch (Exception ex)
            {
                Logger.Write($"绑定锁屏热键失败：{ex.Message}。{ex.StackTrace}");
                Snackbar.Add($"{Lang["ExRegistFailed"]}{ex.Message}", Severity.Error);
            }
        }
    }


    private async Task SetPassword()
    {
        var noHeader = new DialogOptions()
        {
            ClassBackground = "dialog-blurry",
            CloseOnEscapeKey = false,
            CloseButton = true
        };
        var dialog = await Dialog.ShowAsync<SetPassword>(Lang["SetPassword"], noHeader);
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
        AppSettings.IsPasswordChanged = true;
        SaveSettings();
    }
}