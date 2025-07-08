using System.Runtime.InteropServices;

namespace ComputerLock.Platforms;
internal class MouseHook : WindowsInputHook
{
    private int _cursorCount = 0;
    private Random _random = new Random();
    private bool _isAutoInput;
    protected override int HookType => WinApi.WH_MOUSE_LL;

    public event EventHandler? OnUserInput;
    protected override int HookCallback(int nCode, int wParam, IntPtr lParam)
    {
        if (nCode >= 0)
        {
            // 处理鼠标事件
            HandleMouseEvent(wParam);
        }
        return WinApi.CallNextHookEx(_hookId, nCode, wParam, lParam);
    }

    /// <summary>
    /// 隐藏鼠标光标。
    /// </summary>
    public void HideCursor()
    {
        if (_cursorCount >= 0) // 如果光标可见
        {
            _cursorCount = WinApi.ShowCursor(false); // 隐藏光标
        }
    }

    /// <summary>
    /// 显示鼠标光标。
    /// </summary>
    public void ShowCursor()
    {
        if (_cursorCount < 0) // 如果光标不可见
        {
            _cursorCount = WinApi.ShowCursor(true); // 显示光标
        }
    }

    /// <summary>
    /// 重置光标显示状态。
    /// </summary>
    public void ResetCursorState()
    {
        while (_cursorCount < 0) // 如果光标隐藏
        {
            WinApi.ShowCursor(true); // 显示光标
            _cursorCount++;
        }

        while (_cursorCount > 0) // 如果光标多次显示
        {
            WinApi.ShowCursor(false); // 隐藏光标
            _cursorCount--;
        }
    }

    /// <summary>
    /// 移动鼠标并点击
    /// </summary>
    public void MoveAndClick()
    {
        _isAutoInput = true;
        var x = _random.Next(0, 100);
        var y = _random.Next(0, 100);

        var p = new Point(x, y);
        WinApi.SetCursorPos((int)p.X, (int)p.Y);
        WinApi.mouse_event(WinApi.MOUSEEVENTF_RIGHTDOWN | WinApi.MOUSEEVENTF_RIGHTUP, x, y, 0, 0);
        _isAutoInput = false;
    }

    /// <summary>
    /// 处理鼠标事件
    /// </summary>
    private void HandleMouseEvent(int wParam)
    {
        if (_isAutoInput)
        {
            return;
        }

        // 只处理鼠标按下和释放事件
        if (wParam == WinApi.WM_LBUTTONDOWN || wParam == WinApi.WM_LBUTTONUP ||
            wParam == WinApi.WM_RBUTTONDOWN || wParam == WinApi.WM_RBUTTONUP)
        {
            OnUserInput?.Invoke(this, EventArgs.Empty);
        }
    }
}