using ComputerLock.Interfaces;

namespace ComputerLock.Components.Settings;

public partial class LockSettings
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
    private IGlobalLockService GlobalLockService { get; set; } = null!;

    [Inject]
    private HotkeyHook HotkeyHook { get; set; } = null!;


    [Inject]
    private ILogger Logger { get; set; } = null!;

    private void SaveSettings()
    {
        AppSettingsProvider.SaveSettings(AppSettings);
    }

    private void LockStatusDisplayChanged(LockStatusDisplay lockStatusDisplay)
    {
        AppSettings.LockStatusDisplay = lockStatusDisplay;
        SaveSettings();
    }

    private void AutoLockChanged(int autoLockMinute)
    {
        AppSettings.AutoLockSecond = autoLockMinute * 60;
        SaveSettings();
        GlobalLockService.UpdateAutoLockSettings();
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
}