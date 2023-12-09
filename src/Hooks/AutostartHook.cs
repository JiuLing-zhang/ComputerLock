using Microsoft.Win32;

namespace ComputerLock.Hooks;
internal class AutostartHook
{
    private const string RegKey = @"Software\Microsoft\Windows\CurrentVersion\Run";
    public bool IsAutostart()
    {
        var registryKey = Registry.CurrentUser.OpenSubKey(RegKey);
        if (registryKey == null)
        {
            return false;
        }
        if (registryKey.GetValue(AppBase.FriendlyName) == null)
        {
            return false;
        }
        return true;
    }

    public void EnabledAutostart()
    {
        string execPath = AppBase.ExecutablePath;
        var registryKey = Registry.CurrentUser.CreateSubKey(RegKey);
        registryKey.SetValue(AppBase.FriendlyName, $"\"{execPath}\"");
        registryKey.Close();
    }

    public void DisabledAutostart()
    {
        var registryKey = Registry.CurrentUser.CreateSubKey(RegKey);
        registryKey.DeleteValue(AppBase.FriendlyName);
        registryKey.Close();
    }
}