using Microsoft.Win32;

namespace ComputerLock.Hooks
{
    internal class AutostartHook
    {
        private const string RegKey = @"Software\WOW6432Node\Microsoft\Windows\CurrentVersion\Run";
        public static bool IsAutostart()
        {
            var registryKey = Registry.LocalMachine.OpenSubKey(RegKey);
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
            var registryKey = Registry.LocalMachine.CreateSubKey(RegKey);
            registryKey.SetValue(AppBase.FriendlyName, $"\"{execPath}\"");
            registryKey.Close();
        }

        public static void DisabledAutostart()
        {
            var registryKey = Registry.LocalMachine.CreateSubKey(RegKey);
            registryKey.DeleteValue(AppBase.FriendlyName);
            registryKey.Close();
        }
    }
}
