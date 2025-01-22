using System.Runtime.InteropServices;

namespace ComputerLock.Hooks;

/// <summary>
/// 快捷键钩子
/// </summary>
public class HotKeyHook : IDisposable
{
    [DllImport("user32.dll")]
    private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

    [DllImport("user32.dll")]
    private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

    private const int HotkeyId = 90;
    private bool _isRegistered;

    public event Action? HotKeyPressed;

    private sealed class HotKeyNativeWindow : NativeWindow
    {
        public event Action? OnHotKeyPressed;

        public HotKeyNativeWindow()
        {
            this.CreateHandle(new CreateParams());
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0x0312) // WM_HOTKEY
            {
                if (m.WParam.ToInt32() == HotkeyId)
                {
                    OnHotKeyPressed?.Invoke();
                }
            }
            else
            {
                base.WndProc(ref m);
            }
        }
    }

    private readonly HotKeyNativeWindow _nativeWindow;

    public HotKeyHook()
    {
        _nativeWindow = new HotKeyNativeWindow();
        _nativeWindow.OnHotKeyPressed += () => HotKeyPressed?.Invoke();
    }

    /// <summary>
    /// 注册快捷键
    /// </summary>
    public void Register(HotKey hotKey)
    {
        if (_isRegistered)
        {
            Unregister();
        }

        var success = RegisterHotKey(_nativeWindow.Handle, HotkeyId, (uint)hotKey.Modifiers, (uint)hotKey.Key);
        if (!success)
        {
            throw new Exception("注册快捷键失败");
        }
        _isRegistered = success;
    }

    /// <summary>
    /// 注销快捷键
    /// </summary>
    public void Unregister()
    {
        if (!_isRegistered)
        {
            return;
        }
        UnregisterHotKey(_nativeWindow.Handle, HotkeyId);
        _isRegistered = false;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposing)
        {
            return;
        }

        Unregister();
        _nativeWindow.DestroyHandle();
    }

    ~HotKeyHook()
    {
        Dispose(false);
    }
}
