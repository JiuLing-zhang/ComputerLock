namespace ComputerLock.Platforms;
internal class PowerManager
{
    private void AdjustPrivileges()
    {
        WinApi.OpenProcessToken(System.Diagnostics.Process.GetCurrentProcess().Handle,
            WinApi.TOKEN_ADJUST_PRIVILEGES | WinApi.TOKEN_QUERY, out IntPtr htok);

        WinApi.LookupPrivilegeValue(null, WinApi.SE_SHUTDOWN_NAME, out var luid);

        var tp = new WinApi.TOKEN_PRIVILEGES
        {
            PrivilegeCount = 1,
            Privileges = new WinApi.LUID_AND_ATTRIBUTES[1]
        };
        tp.Privileges[0].Luid = luid;
        tp.Privileges[0].Attributes = WinApi.SE_PRIVILEGE_ENABLED;

        WinApi.AdjustTokenPrivileges(htok, false, ref tp, 0, IntPtr.Zero, IntPtr.Zero);
    }

    public void Shutdown()
    {
        AdjustPrivileges();
        WinApi.ExitWindowsEx(WinApi.EWX_SHUTDOWN, 0);
    }

    public void Hibernate()
    {
        AdjustPrivileges();
        WinApi.SetSuspendState(true, true, true);
    }

    public bool IsHibernateSupported()
    {
        // 简单检查系统是否支持休眠
        string? sysDrive = Environment.GetEnvironmentVariable("SystemDrive");
        if (sysDrive == null)
        {
            return false;
        }
        string hiberFile = System.IO.Path.Combine(sysDrive, "hiberfil.sys");
        if (!System.IO.File.Exists(hiberFile))
        {
            return false;
        }
        return true;
    }
}