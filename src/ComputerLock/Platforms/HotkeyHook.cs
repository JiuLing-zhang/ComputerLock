namespace ComputerLock.Platforms;

/// <summary>
/// 快捷键钩子
/// </summary>
public class HotkeyHook : IDisposable
{
    private List<int> ids = new List<int>();

    public event Action<int>? HotkeyPressed;

    private sealed class HotkeyNativeWindow : NativeWindow
    {
        public event Action<int>? OnHotkeyPressed;

        public HotkeyNativeWindow()
        {
            this.CreateHandle(new CreateParams());
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0x0312) // WM_HOTKEY
            {
                OnHotkeyPressed?.Invoke(m.WParam.ToInt32());
            }
            else
            {
                base.WndProc(ref m);
            }
        }
    }

    private readonly HotkeyNativeWindow _nativeWindow;

    public HotkeyHook()
    {
        _nativeWindow = new HotkeyNativeWindow();
        _nativeWindow.OnHotkeyPressed += (id) => HotkeyPressed?.Invoke(id);
    }

    /// <summary>
    /// 注册快捷键
    /// </summary>
    public void Register(int id, Hotkey hotKey)
    {
        if (ids.Contains(id))
        {
            Unregister(id);
        }

        var success = WinApi.RegisterHotKey(_nativeWindow.Handle, id, (uint)hotKey.Modifiers, (uint)hotKey.Key);
        if (!success)
        {
            throw new Exception($"注册快捷键失败。id({id})");
        }

        ids.Add(id);
    }

    /// <summary>
    /// 注销快捷键
    /// </summary>
    public void Unregister(int id)
    {
        WinApi.UnregisterHotKey(_nativeWindow.Handle, id);
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

        foreach (var id in ids)
        {
            Unregister(id);
        }
        _nativeWindow.DestroyHandle();
    }

    ~HotkeyHook()
    {
        Dispose(false);
    }
}
