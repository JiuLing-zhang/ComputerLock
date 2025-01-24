using System.Runtime.InteropServices;

namespace ComputerLock.Platforms;
internal class MouseHook
{
    [DllImport("user32.dll")]
    private static extern int ShowCursor(bool bShow);

    private int _cursorCount = 0;

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
}