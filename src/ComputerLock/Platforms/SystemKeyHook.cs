using System.Runtime.InteropServices;

namespace ComputerLock.Platforms;
/// <summary>
/// 系统按键钩子，用于禁用系统按键
/// </summary>
internal class SystemKeyHook : WindowsInputHook
{
    private Hotkey? _ignoreHotkey;   // 使用单个Hotkey变量来存储需要忽略的快捷键

    protected override int HookType => WinApi.WH_KEYBOARD_LL;

    public event EventHandler? OnUserInput;

    public void SetIgnoreHotkey(Hotkey? hotKey)
    {
        _ignoreHotkey = hotKey;
    }

    protected override int HookCallback(int nCode, int wParam, IntPtr lParam)
    {
        if (nCode < 0 || !(wParam == WinApi.WM_KEYDOWN || wParam == WinApi.WM_SYSKEYDOWN))
        {
            return WinApi.CallNextHookEx(_hookId, nCode, wParam, lParam);
        }

        int vkCode = Marshal.ReadInt32(lParam);

        if (_ignoreHotkey == null)
        {
            if (IsBlockedSystemKey(vkCode))
            {
                OnUserInput?.Invoke(this, EventArgs.Empty);
                return 1;
            }

            return WinApi.CallNextHookEx(_hookId, nCode, wParam, lParam);
        }

        if (IsPartOfIgnoreHotkey(vkCode))
        {
            return WinApi.CallNextHookEx(_hookId, nCode, wParam, lParam);
        }

        OnUserInput?.Invoke(this, EventArgs.Empty);
        return 1;
    }

    #region 快捷键辅助方法

    private bool IsPartOfIgnoreHotkey(int vkCode)
    {
        bool isPart = false;

        if (_ignoreHotkey!.Modifiers.HasFlag(HotkeyModifiers.Control))
        {
            isPart |= (vkCode == WinApi.VK_LCONTROL || vkCode == WinApi.VK_RCONTROL);
        }

        if (_ignoreHotkey.Modifiers.HasFlag(HotkeyModifiers.Shift))
        {
            isPart |= (vkCode == WinApi.VK_LSHIFT || vkCode == WinApi.VK_RSHIFT);
        }

        if (_ignoreHotkey.Modifiers.HasFlag(HotkeyModifiers.Alt))
        {
            isPart |= (vkCode == WinApi.VK_LMENU || vkCode == WinApi.VK_RMENU);
        }

        isPart |= (vkCode == (int)_ignoreHotkey.Key);

        return isPart;
    }

    #endregion

    #region 密码解锁辅助方法

    /// <summary>
    /// 拦截单独的系统键
    /// </summary>
    private bool IsBlockedSystemKey(int vkCode)
    {
        return vkCode == WinApi.VK_LWIN || vkCode == WinApi.VK_RWIN || // Win键
               vkCode == WinApi.VK_LCONTROL || vkCode == WinApi.VK_RCONTROL || // Ctrl
               vkCode == WinApi.VK_LSHIFT || vkCode == WinApi.VK_RSHIFT || // Shift
               vkCode == WinApi.VK_LMENU || vkCode == WinApi.VK_RMENU || // Alt
               vkCode == WinApi.VK_TAB;                                  // Tab
    }
    #endregion
}
