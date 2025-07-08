using System.Diagnostics;

namespace ComputerLock.Platforms;

/// <summary>
/// Windows 系统输入钩子基类
/// </summary>
internal abstract class WindowsInputHook : IDisposable
{
    protected int _hookId;
    protected readonly WinApi.HookDelegate _hookCallback;

    protected WindowsInputHook()
    {
        _hookCallback = HookCallback;
    }

    protected abstract int HookType { get; }

    protected abstract int HookCallback(int nCode, int wParam, IntPtr lParam);

    /// <summary>
    /// 安装系统钩子
    /// </summary>
    public void InstallHook()
    {
        using Process curProcess = Process.GetCurrentProcess();
        string? moduleName = curProcess.MainModule?.ModuleName;
        if (moduleName == null)
        {
            return;
        }
        _hookId = WinApi.SetWindowsHookEx(HookType, _hookCallback, WinApi.GetModuleHandle(moduleName), 0);
    }

    public void Dispose()
    {
        WinApi.UnhookWindowsHookEx(_hookId);
    }
}