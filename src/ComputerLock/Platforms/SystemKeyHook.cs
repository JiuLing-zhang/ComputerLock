using System.Diagnostics;
using System.Runtime.InteropServices;

namespace ComputerLock.Platforms;
internal class SystemKeyHook : IDisposable
{
    // ReSharper disable InconsistentNaming
    private const int WH_KEYBOARD_LL = 13;
    private const int WM_KEYDOWN = 0x0100;
    private const int WM_SYSKEYDOWN = 0x0104;

    private const int VK_LWIN = 0x5B;
    private const int VK_RWIN = 0x5C;
    private const int VK_LSHIFT = 0xA0;
    private const int VK_RSHIFT = 0xA1;
    private const int VK_LCONTROL = 0xA2;
    private const int VK_RCONTROL = 0xA3;
    //ALT
    private const int VK_LMENU = 0xA4;
    private const int VK_RMENU = 0xA5;
    private const int VK_TAB = 0x09;
    // ReSharper restore InconsistentNaming

    private int _hookId;
    private readonly HookDelegate _hookCallback;
    private Hotkey? _ignoreHotkey; // 使用单个Hotkey变量来存储需要忽略的快捷键

    public delegate int HookDelegate(int nCode, int wParam, IntPtr lParam);

    [DllImport("user32.dll")]
    public static extern int SetWindowsHookEx(int idHook, HookDelegate callback, IntPtr hInstance, uint threadId);

    [DllImport("user32.dll")]
    public static extern int UnhookWindowsHookEx(int idHook);

    [DllImport("user32.dll")]
    public static extern int CallNextHookEx(int idHook, int nCode, int wParam, IntPtr lParam);

    [DllImport("kernel32.dll")]
    public static extern IntPtr GetModuleHandle(string lpModuleName);


    public SystemKeyHook()
    {
        _hookCallback = KeyboardHookCallback;
    }

    public void DisableSystemKey()
    {
        using Process curProcess = Process.GetCurrentProcess();
        string? moduleName = curProcess.MainModule?.ModuleName;
        if (moduleName == null)
        {
            return;
        }
        _hookId = SetWindowsHookEx(WH_KEYBOARD_LL, _hookCallback, GetModuleHandle(moduleName), 0);
    }

    public void SetIgnoreHotkey(Hotkey hotKey)
    {
        _ignoreHotkey = hotKey;
    }
    private int KeyboardHookCallback(int nCode, int wParam, IntPtr lParam)
    {
        if (nCode >= 0)
        {
            int vkCode = Marshal.ReadInt32(lParam);

            if (_ignoreHotkey == null)
            {
                if (IsSystemKey(vkCode) && (wParam == WM_KEYDOWN || wParam == WM_SYSKEYDOWN))
                {
                    return 1; // 阻止事件传递
                }
                return CallNextHookEx(_hookId, nCode, wParam, lParam); // 其他按键放行
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
            return CallNextHookEx(_hookId, nCode, wParam, lParam); // 放行
        }
        return CallNextHookEx(_hookId, nCode, wParam, lParam);
    }

    private bool IsSystemKey(int vkCode)
    {
        return vkCode == VK_LWIN || vkCode == VK_RWIN ||
               vkCode == VK_LSHIFT || vkCode == VK_RSHIFT ||
               vkCode == VK_LCONTROL || vkCode == VK_RCONTROL ||                
               vkCode == VK_TAB;
    }

    private bool IsPartOfIgnoreHotkey(int vkCode)
    {
        bool isPartOfIgnoreHotkey = false;
        if (_ignoreHotkey!.Modifiers.HasFlag(HotkeyModifiers.Control))
        {
            isPartOfIgnoreHotkey |= (vkCode == VK_LCONTROL || vkCode == VK_RCONTROL);
        }

        if (_ignoreHotkey.Modifiers.HasFlag(HotkeyModifiers.Shift))
        {
            isPartOfIgnoreHotkey |= (vkCode == VK_LSHIFT || vkCode == VK_RSHIFT);
        }

        if (_ignoreHotkey.Modifiers.HasFlag(HotkeyModifiers.Alt))
        {
            isPartOfIgnoreHotkey |= (vkCode == VK_LMENU || vkCode == VK_RMENU);
        }

        isPartOfIgnoreHotkey |= (vkCode == (int)_ignoreHotkey.Key);
        return isPartOfIgnoreHotkey;
    }

    private bool IsModifierKey(int vkCode)
    {
        return vkCode == VK_LCONTROL || vkCode == VK_RCONTROL ||
               vkCode == VK_LSHIFT || vkCode == VK_RSHIFT ||
               vkCode == VK_LMENU || vkCode == VK_RMENU;
    }

    private bool IsModifierRequired(int vkCode)
    {
        if (vkCode == VK_LCONTROL || vkCode == VK_RCONTROL)
        {
            return _ignoreHotkey!.Modifiers.HasFlag(HotkeyModifiers.Control);
        }
        if (vkCode == VK_LSHIFT || vkCode == VK_RSHIFT)
        {
            return _ignoreHotkey!.Modifiers.HasFlag(HotkeyModifiers.Shift);
        }
        if (vkCode == VK_LMENU || vkCode == VK_RMENU)
        {
            return _ignoreHotkey!.Modifiers.HasFlag(HotkeyModifiers.Alt);
        }
        return false;
    }

    public void Dispose()
    {
        _ignoreHotkey = null;
        UnhookWindowsHookEx(_hookId);
    }
}