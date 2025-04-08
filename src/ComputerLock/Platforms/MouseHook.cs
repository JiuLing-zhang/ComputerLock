using System.Runtime.InteropServices;

namespace ComputerLock.Platforms;
internal class MouseHook
{
    [DllImport("user32.dll")]
    private static extern int ShowCursor(bool bShow);

    [DllImport("user32.dll")]
    private static extern int SetCursorPos(int x, int y);

    [DllImport("user32.dll")]
    private static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);

    public const int MOUSEEVENTF_LEFTDOWN = 0x0002;
    public const int MOUSEEVENTF_LEFTUP = 0x0004;
    public const int MOUSEEVENTF_RIGHTDOWN = 0x0008;
    public const int MOUSEEVENTF_RIGHTUP = 0x0010;

    private int _cursorCount = 0;
    private Random _random = new Random();

    /// <summary>
    /// 隐藏鼠标光标。
    /// </summary>
    public void HideCursor()
    {
        if (_cursorCount >= 0) // 如果光标可见
        {
            _cursorCount = ShowCursor(false); // 隐藏光标
        }
    }

    /// <summary>
    /// 显示鼠标光标。
    /// </summary>
    public void ShowCursor()
    {
        if (_cursorCount < 0) // 如果光标不可见
        {
            _cursorCount = ShowCursor(true); // 显示光标
        }
    }

    /// <summary>
    /// 重置光标显示状态。
    /// </summary>
    public void ResetCursorState()
    {
        while (_cursorCount < 0) // 如果光标隐藏
        {
            ShowCursor(true); // 显示光标
            _cursorCount++;
        }

        while (_cursorCount > 0) // 如果光标多次显示
        {
            ShowCursor(false); // 隐藏光标
            _cursorCount--;
        }
    }

    /// <summary>
    /// 移动鼠标并点击
    /// </summary>
    public void MoveAndClick()
    {
        var x = _random.Next(0, 100);
        var y = _random.Next(0, 100);

        var p = new Point(x, y);
        SetCursorPos((int)p.X, (int)p.Y);
        mouse_event(MOUSEEVENTF_RIGHTDOWN | MOUSEEVENTF_RIGHTUP, x, y, 0, 0);
    }
}