using System.IO;
using System.Windows;
using ComputerLock.Hooks;
using ComputerLock.Platforms;
using ComputerLock.Resources;

namespace ComputerLock;
public partial class WindowMain : Window
{
    private readonly KeyboardHook _keyboardHook;
    private readonly AppSettings _appSettings;
    private readonly ILocker _locker;

    private NotifyIcon _notifyIcon;
    private ContextMenuStrip _contextMenuStrip;

    public WindowMain(KeyboardHook keyboardHook, AppSettings appSettings, ILocker locker)
    {
        InitializeComponent();
        _keyboardHook = keyboardHook;
        _appSettings = appSettings;
        _locker = locker;
        InitializeNotifyIcon();

    }
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        this.WindowState = _appSettings.IsHideWindowWhenLaunch ? WindowState.Minimized : WindowState.Normal;
    }

    private void InitializeNotifyIcon()
    {
        _notifyIcon = new NotifyIcon();
        _contextMenuStrip = new ContextMenuStrip();

        var btnShowWindow = new ToolStripMenuItem(Lang.ShowMainWindow);
        btnShowWindow.Click += (_, __) => ShowMainWindow();
        _contextMenuStrip.Items.Add(btnShowWindow);

        var btnLock = new ToolStripMenuItem(Lang.DoLock);
        btnLock.Click += (_, __) => _locker.Lock();
        _contextMenuStrip.Items.Add(btnLock);

        var btnClose = new ToolStripMenuItem(Lang.Exit);
        btnClose.Click += (_, __) =>
        {
            Dispose();
            System.Windows.Application.Current.Shutdown();
        };
        _contextMenuStrip.Items.Add(btnClose);

        _notifyIcon.ContextMenuStrip = _contextMenuStrip;
        Stream iconStream = System.Windows.Application.GetResourceStream(new Uri("pack://application:,,,/ComputerLock;component/icon.ico")).Stream;
        _notifyIcon.Icon = new System.Drawing.Icon(iconStream);
        _notifyIcon.Click += (object? sender, EventArgs e) =>
        {
            var args = e as MouseEventArgs;
            if (args is not { Button: MouseButtons.Left })
            {
                return;
            }
            ShowMainWindow();
        };
        _notifyIcon.Visible = true;
    }

    private void ShowMainWindow()
    {
        this.ShowInTaskbar = true;
        this.WindowState = WindowState.Normal;
        this.Activate();
    }

    private void Window_StateChanged(object sender, EventArgs e)
    {
        if (this.WindowState == WindowState.Minimized)
        {
            this.ShowInTaskbar = false;
        }
    }

    private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
        if (_appSettings.IsHideWindowWhenClose)
        {
            this.WindowState = WindowState.Minimized;
            e.Cancel = true;
            return;
        }
        Dispose();
    }

    private void Window_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
    {

        if (e.Key == System.Windows.Input.Key.Escape)
        {
            this.WindowState = WindowState.Minimized;
            this.ShowInTaskbar = false;
        }
    }
    public void Dispose()
    {
        _notifyIcon.Visible = false;
        _keyboardHook.Dispose();
    }
}