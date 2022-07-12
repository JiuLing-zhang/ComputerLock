using Microsoft.Win32;

namespace ComputerLock.Hooks
{
    internal class AutostartHook
    {
        public static bool IsAutostart()
        {
            var key = "Software\\Microsoft\\Windows\\CurrentVersion\\Run";
            var registryKey = Registry.CurrentUser.OpenSubKey(key);
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

        public static void EnabledAutostart()
        {
            string execPath = AppBase.ExecutablePath;
            var key = "Software\\Microsoft\\Windows\\CurrentVersion\\Run";
            var registryKey = Registry.CurrentUser.CreateSubKey(key);
            registryKey.SetValue(AppBase.FriendlyName, execPath);
            registryKey.Close();
        }

        public static void DisabledAutostart()
        {
            var key = "Software\\Microsoft\\Windows\\CurrentVersion\\Run";
            var registryKey = Registry.CurrentUser.CreateSubKey(key);
            registryKey.DeleteValue(AppBase.FriendlyName);
            registryKey.Close();
        }
    }
}
