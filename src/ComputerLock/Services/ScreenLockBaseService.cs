using ComputerLock.Interfaces;
using System.Windows;
using System.Windows.Threading;
using Application = System.Windows.Application;

namespace ComputerLock.Services;

public abstract class ScreenLockBaseService : IScreenLockService
{
    public abstract event EventHandler? OnUnlock;
    public abstract bool Lock(bool showAnimation);
    public abstract void Unlock();

    protected void ShowWindowOnScreen(Window window, Screen screen)
    {
        // 获取包括任务栏的完整屏幕区域
        var bounds = screen.Bounds;

        // 设置窗口初始位置和大小
        window.WindowStartupLocation = WindowStartupLocation.Manual;
        window.Left = bounds.Left;
        window.Top = bounds.Top;
        window.Width = bounds.Width;
        window.Height = bounds.Height;

        // 在窗口加载后，根据屏幕的 DPI 重新调整位置和大小
        // 必须先显示窗口，然后才能获取 DPI，所以窗口大小和位置需要二次调整
        window.Loaded += (_, _) =>
        {
            var dpiFactor = GetDpiFactor(window);
            window.Left = bounds.Left / dpiFactor.X;
            window.Top = bounds.Top / dpiFactor.Y;
            window.Width = bounds.Width / dpiFactor.X;
            window.Height = bounds.Height / dpiFactor.Y;
        };

        window.WindowStyle = WindowStyle.None;
        window.ResizeMode = System.Windows.ResizeMode.NoResize;

        window.Show();
        window.Activate();
    }

    private (double X, double Y) GetDpiFactor(Window window)
    {
        var source = PresentationSource.FromVisual(window);
        if (source?.CompositionTarget != null)
        {
            var transform = source.CompositionTarget.TransformToDevice;
            return (transform.M11, transform.M22);
        }
        return (1.0, 1.0); // 默认比例
    }

    protected void ShowPopup(string message)
    {
        Application.Current.Dispatcher.Invoke(() =>
        {
            var popup = new WindowPopup(message);
            double primaryScreenWidth = SystemParameters.PrimaryScreenWidth;
            double primaryScreenHeight = SystemParameters.PrimaryScreenHeight;
            popup.Left = (primaryScreenWidth - popup.Width) / 2;
            popup.Top = (primaryScreenHeight - popup.Height) / 2;
            popup.Show();

            var timer = new DispatcherTimer()
            {
                Interval = TimeSpan.FromMilliseconds(1100),
            };
            timer.Tick += (_, _) =>
            {
                timer.Stop();
                popup.CloseWindow();
            };
            timer.Start();
        });
    }
}