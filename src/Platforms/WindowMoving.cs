using System.Runtime.InteropServices;
using System.Windows.Threading;
using Application = System.Windows.Application;

namespace ComputerLock.Platforms;
internal class WindowMoving : IWindowMoving
{
    private bool _isMoving;

    private double _mouseStartX;
    private double _mouseStartY;
    private double _windowStartLeft;
    private double _windowStartTop;

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static extern bool GetCursorPos(ref Win32Point pt);

    [StructLayout(LayoutKind.Sequential)]
    internal struct Win32Point
    {
        public Int32 X;
        public Int32 Y;
    };

    public WindowMoving()
    {
        var timer = new DispatcherTimer();
        timer.Interval = TimeSpan.FromMilliseconds(10);
        timer.Tick += (_, __) =>
        {
            if (!_isMoving)
            {
                return;
            }
            var point = GetMousePosition();
            Application.Current.MainWindow.Left = _windowStartLeft - _mouseStartX + point.X;
            Application.Current.MainWindow.Top = _windowStartTop - _mouseStartY + point.Y;
        };
        timer.Start();
    }

    public void MouseDown()
    {
        var point = GetMousePosition();
        _mouseStartX = point.X;
        _mouseStartY = point.Y;
        _windowStartLeft = Application.Current.MainWindow.Left;
        _windowStartTop = Application.Current.MainWindow.Top;
        _isMoving = true;
    }

    public static System.Windows.Point GetMousePosition()
    {
        var w32Mouse = new Win32Point();
        GetCursorPos(ref w32Mouse);

        return new System.Windows.Point(w32Mouse.X, w32Mouse.Y);
    }
    public void MouseUp()
    {
        _isMoving = false;
    }
}