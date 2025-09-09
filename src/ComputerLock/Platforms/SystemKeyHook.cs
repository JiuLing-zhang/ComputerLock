using System.Runtime.InteropServices;

namespace ComputerLock.Platforms;
/// <summary>
/// 系统按键钩子，用于禁用系统按键
/// </summary>
internal class SystemKeyHook : WindowsInputHook
{
    private Hotkey? _ignoreHotkey; // 使用单个Hotkey变量来存储需要忽略的快捷键

    protected override int HookType => WinApi.WH_KEYBOARD_LL;

    public event EventHandler? OnUserInput;

    public void SetIgnoreHotkey(Hotkey? hotKey)
    {
        _ignoreHotkey = hotKey;
    }

    protected override int HookCallback(int nCode, int wParam, IntPtr lParam)
    {
        if (nCode >= 0)
        {
            int vkCode = Marshal.ReadInt32(lParam);

            if (_ignoreHotkey == null)
            {
                if (IsSystemKey(vkCode) && (wParam == WinApi.WM_KEYDOWN || wParam == WinApi.WM_SYSKEYDOWN))
                {
                    OnUserInput?.Invoke(this, EventArgs.Empty);
                    return 1; // 阻止事件传递
                }
                return WinApi.CallNextHookEx(_hookId, nCode, wParam, lParam); // 其他按键放行
            }

            if (!IsPartOfIgnoreHotkey(vkCode))
            {
                if (IsModifierKey(vkCode) && !IsModifierRequired(vkCode))
                {
                    return 1; // 阻止事件传递
                }
                else if (vkCode != (int)_ignoreHotkey.Key)
                {
                    return 1; // 阻止事件传递
                }
            }
            return WinApi.CallNextHookEx(_hookId, nCode, wParam, lParam); // 放行
        }
        return WinApi.CallNextHookEx(_hookId, nCode, wParam, lParam);
    }

    private bool IsSystemKey(int vkCode)
    {
        return vkCode == WinApi.VK_LWIN || vkCode == WinApi.VK_RWIN ||
               vkCode == WinApi.VK_LSHIFT || vkCode == WinApi.VK_RSHIFT ||
               vkCode == WinApi.VK_LCONTROL || vkCode == WinApi.VK_RCONTROL ||
               vkCode == WinApi.VK_TAB;
    }

    private bool IsPartOfIgnoreHotkey(int vkCode)
    {
        bool isPartOfIgnoreHotkey = false;
        if (_ignoreHotkey!.Modifiers.HasFlag(HotkeyModifiers.Control))
        {
            isPartOfIgnoreHotkey |= (vkCode == WinApi.VK_LCONTROL || vkCode == WinApi.VK_RCONTROL);
        }

        if (_ignoreHotkey.Modifiers.HasFlag(HotkeyModifiers.Shift))
        {
            isPartOfIgnoreHotkey |= (vkCode == WinApi.VK_LSHIFT || vkCode == WinApi.VK_RSHIFT);
        }

        if (_ignoreHotkey.Modifiers.HasFlag(HotkeyModifiers.Alt))
        {
            isPartOfIgnoreHotkey |= (vkCode == WinApi.VK_LMENU || vkCode == WinApi.VK_RMENU);
        }

        isPartOfIgnoreHotkey |= (vkCode == (int)_ignoreHotkey.Key);
        return isPartOfIgnoreHotkey;
    }

    private bool IsModifierKey(int vkCode)
    {
        return vkCode == WinApi.VK_LCONTROL || vkCode == WinApi.VK_RCONTROL ||
               vkCode == WinApi.VK_LSHIFT || vkCode == WinApi.VK_RSHIFT ||
               vkCode == WinApi.VK_LMENU || vkCode == WinApi.VK_RMENU;
    }

    private bool IsModifierRequired(int vkCode)
    {
        if (vkCode == WinApi.VK_LCONTROL || vkCode == WinApi.VK_RCONTROL)
        {
            return _ignoreHotkey!.Modifiers.HasFlag(HotkeyModifiers.Control);
        }
        if (vkCode == WinApi.VK_LSHIFT || vkCode == WinApi.VK_RSHIFT)
        {
            return _ignoreHotkey!.Modifiers.HasFlag(HotkeyModifiers.Shift);
        }
        if (vkCode == WinApi.VK_LMENU || vkCode == WinApi.VK_RMENU)
        {
            return _ignoreHotkey!.Modifiers.HasFlag(HotkeyModifiers.Alt);
        }
        return false;
    }
}