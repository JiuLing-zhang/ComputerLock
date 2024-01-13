using System.Windows;
using Application = System.Windows.Application;

namespace ComputerLock.Platforms;
internal class WindowTitleBar : IWindowTitleBar
{
    public bool IsMaximized => Application.Current.MainWindow.WindowState == WindowState.Maximized;

    public void Minimize()
    {
        if (Application.Current.MainWindow == null)
        {
            return;
        }
        Application.Current.MainWindow.WindowState = WindowState.Minimized;
    }

    public void Maximize()
    {
        if (Application.Current.MainWindow == null)
        {
            return;
        }

        if (IsMaximized)
        {
            Application.Current.MainWindow.WindowState = WindowState.Normal;
        }
        else
        {
            Application.Current.MainWindow.WindowState = WindowState.Maximized;
        }
    }

    public void Close()
    {
        if (Application.Current.MainWindow == null)
        {
            return;
        }
        Application.Current.MainWindow.Close();
    }

    public void Restart()
    {
        if (Application.Current.MainWindow == null)
        {
            return;
        }
        Application.Current.Shutdown();
        System.Windows.Forms.Application.Restart();
    }
}