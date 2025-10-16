using ComputerLock.Interfaces;
using Microsoft.Win32;

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

    [Inject]
    private PowerManager PowerManager { get; set; } = null!;
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
        // 检查快捷键是否与解锁快捷键相同
        if (hotkey.IsNotEmpty() && hotkey == AppSettings.UnlockHotkeyString)
        {
            Snackbar.Add(Lang["HotkeyDuplicateError"], Severity.Error);
            return Task.CompletedTask;
        }

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
                Logger.Info("注册锁屏热键");
                HotkeyHook.Register((int)HotkeyType.Lock, AppSettings.LockHotkey);
            }
        }
        catch (Exception ex)
        {
            Logger.Error($"绑定锁屏热键失败", ex);
            Snackbar.Add($"{Lang["ExRegistFailed"]}{ex.Message}", Severity.Error);
        }
    }
    public void UnregisterLockHotkey()
    {
        try
        {
            Logger.Info("释放锁屏热键");
            HotkeyHook.Unregister((int)HotkeyType.Lock);
        }
        catch (Exception ex)
        {
            Logger.Error($"释放锁屏热键失败", ex);
            //MessageBoxUtils.ShowError($"取消快捷键失败：{ex.Message}");
        }
    }

    private void AutoPowerChanged(int autoPowerMinute)
    {
        AppSettings.AutoPowerSecond = autoPowerMinute * 60;
        SaveSettings();
    }

    private void PowerActionTypeChanged(PowerActionType powerActionType)
    {
        if (powerActionType == PowerActionType.Hibernate && !PowerManager.IsHibernateSupported())
        {
            Snackbar.Add("系统可能未启用休眠功能，该功能可能无效。", Severity.Error);
            return;
        }
        AppSettings.AutoPowerActionType = powerActionType;
        SaveSettings();
    }

    private void SetBackgroundColor(string color)
    {
        AppSettings.LockScreenBackgroundColor = color;
        SaveSettings();
    }

    private void SelectBackgroundImage()
    {
        var openFileDialog = new OpenFileDialog
        {
            Filter = "Image files (*.jpg, *.jpeg, *.png, *.bmp)|*.jpg;*.jpeg;*.png;*.bmp|All files (*.*)|*.*",
            Title = "Select Background Image"
        };

        if (openFileDialog.ShowDialog() == true)
        {
            AppSettings.LockScreenBackgroundImage = openFileDialog.FileName;
            SaveSettings();
        }
    }

    private void ClearBackgroundImage()
    {
        AppSettings.LockScreenBackgroundImage = "";
        SaveSettings();
    }
}