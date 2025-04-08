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

        if (AppSettings.LockHotkeyString.IsNotEmpty())
        {
            RegisterLockHotkey();
        }
        if (AppSettings.UnlockHotkeyString.IsNotEmpty())
        {
            RegisterUnlockHotkey();
        }

        HotkeyHook.HotkeyPressed += (id) =>
        {
            if (id == (int)HotkeyType.Lock)
            {
                if (!GlobalLockService.IsLocked)
                {
                    Logger.Write("快捷键锁定");
                    GlobalLockService.Lock();
                }
                else
                {
                    if (AppSettings.ScreenUnlockMethod == ScreenUnlockMethods.Hotkey && AppSettings.IsUnlockUseLockHotkey)
                    {
                        Logger.Write("快捷键解锁");
                        GlobalLockService.Unlock();
                    }
                }
            }
            else if (id == (int)HotkeyType.Unlock)
            {
                if (GlobalLockService.IsLocked)
                {
                    Logger.Write("快捷键解锁（独立解锁）");
                    GlobalLockService.Unlock();
                }
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
        GlobalLockService.UpdateAutoLockSettings();
    }

    private void PwdBoxLocationChanged(ScreenLocationEnum location)
    {
        AppSettings.PasswordInputLocation = location;
        SaveSettings();
    }

    private void LockStatusDisplayChanged(LockStatusDisplay lockStatusDisplay)
    {
        AppSettings.LockStatusDisplay = lockStatusDisplay;
        SaveSettings();
    }

    private void SaveSettings()
    {
        AppSettingsProvider.SaveSettings(AppSettings);
    }
    private Task SetLockHotkey(string hotkey)
    {
        AppSettings.LockHotkeyString = hotkey;
        SaveSettings();
        RegisterLockHotkey();
        return Task.CompletedTask;
    }
    private Task ClearLockHotkey()
    {
        AppSettings.LockHotkeyString = "";
        SaveSettings();
        UnregisterLockHotkey();
        return Task.CompletedTask;
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

    public void RegisterLockHotkey()
    {
        try
        {
            if (AppSettings.LockHotkey != null)
            {
                Logger.Write("注册锁屏热键");
                HotkeyHook.Register((int)HotkeyType.Lock, AppSettings.LockHotkey);
            }
        }
        catch (Exception ex)
        {
            Logger.Write($"绑定锁屏热键失败：{ex.Message}。{ex.StackTrace}");
            Snackbar.Add($"{Lang["ExRegistFailed"]}{ex.Message}", Severity.Error);
        }
    }
    public void UnregisterLockHotkey()
    {
        try
        {
            Logger.Write("释放锁屏热键");
            HotkeyHook.Unregister((int)HotkeyType.Lock);
        }
        catch (Exception ex)
        {
            Logger.Write($"释放锁屏热键失败：{ex.Message}。{ex.StackTrace}");
            //MessageBoxUtils.ShowError($"取消快捷键失败：{ex.Message}");
        }
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
    private async Task ResetPassword()
    {
        var noHeader = new DialogOptions()
        {
            BackgroundClass = "dialog-blurry",
            CloseOnEscapeKey = false,
            CloseButton = true
        };
        IDialogReference dialog;
        if (AppSettings.Password.IsEmpty())
        {
            dialog = await Dialog.ShowAsync<SetPassword>(Lang["ResetPassword"], noHeader);
        }
        else
        {
            dialog = await Dialog.ShowAsync<ResetPassword>(Lang["ResetPassword"], noHeader);
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
}