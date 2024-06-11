using System.IO;
using System.Windows;
using Microsoft.Win32;

namespace ComputerLock;
public partial class WindowMain : Window, IDisposable
{
    private readonly KeyboardHook _keyboardHook;
    private readonly AppSettings _appSettings;
    private readonly UserActivityMonitor? _activityMonitor;
    private readonly ILocker _locker;
    private readonly ILogger _logger;

    private readonly NotifyIcon _notifyIcon = new();
    private readonly ContextMenuStrip _contextMenuStrip = new();

    public WindowMain(KeyboardHook keyboardHook, AppSettings appSettings, ILocker locker, UserActivityMonitor activityMonitor, ILogger logger)
    {
        InitializeComponent();

        _keyboardHook = keyboardHook;
        _appSettings = appSettings;
        _locker = locker;
        _logger = logger;

        InitializeNotifyIcon();
        _logger.Write("系统启动");

        if (_appSettings.AutoLockSecond != 0)
        {
            _logger.Write("自动锁定已生效");
            _activityMonitor = activityMonitor;
            _activityMonitor.Init(_appSettings.AutoLockSecond);
            _activityMonitor.OnIdle += (_, _) =>
            {
                Dispatcher.Invoke(() =>
                {
                    _logger.Write("自动锁定 -> 锁定");
                    _locker.Lock();
                });
            };
            _locker.OnLock += (_, _) =>
            {
                _logger.Write("自动锁定 -> 暂停空闲检测");
                _activityMonitor.StopMonitoring();
            };
            _locker.OnUnlock += (_, _) =>
            {
                _logger.Write("自动锁定 -> 启动空闲检测");
                _activityMonitor.StartMonitoring();
            };
            _logger.Write("自动锁定 -> 启动空闲检测");
            _activityMonitor.StartMonitoring();

            _logger.Write("自动锁定 -> 准备监控系统会话状态");
            SystemEvents.SessionSwitch += (_, e) =>
            {
                if (e.Reason == SessionSwitchReason.SessionLock)
                {
                    _logger.Write("Windows系统锁定 -> 暂停空闲检测");
                    _activityMonitor.StopMonitoring();
                }
                else if (e.Reason == SessionSwitchReason.SessionUnlock)
                {
                    _logger.Write("Windows系统解锁 -> 启动空闲检测");
                    _activityMonitor.StartMonitoring();
                }
            };
        }

        if (_appSettings.LockOnStartup)
        {
            _logger.Write("程序启动时锁定屏幕");
            _locker.Lock();
        }
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        this.Title = Lang.Title;
        this.WindowState = _appSettings.IsHideWindowWhenLaunch ? WindowState.Minimized : WindowState.Normal;
    }

    private void InitializeNotifyIcon()
    {

        var btnShowWindow = new ToolStripMenuItem(Lang.ShowMainWindow);
        btnShowWindow.Click += (_, _) => ShowMainWindow();
        _contextMenuStrip.Items.Add(btnShowWindow);

        var btnLock = new ToolStripMenuItem(Lang.DoLock);
        btnLock.Click += (_, _) =>
        {
            _logger.Write("托盘锁定");
            _locker.Lock();
        };
        _contextMenuStrip.Items.Add(btnLock);

        var btnClose = new ToolStripMenuItem(Lang.Exit);
        btnClose.Click += (_, _) =>
        {
            _logger.Write("托盘关闭");
            System.Windows.Application.Current.Shutdown();
        };
        _contextMenuStrip.Items.Add(btnClose);

        _notifyIcon.ContextMenuStrip = _contextMenuStrip;
        Stream iconStream = System.Windows.Application.GetResourceStream(new Uri("pack://application:,,,/ComputerLock;component/icon.ico")).Stream;
        _notifyIcon.Icon = new Icon(iconStream);
        _notifyIcon.Text = Lang.Title;
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
        }
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
        _logger.Write("系统资源释放，系统关闭");
        _notifyIcon.Dispose();
        _activityMonitor?.Dispose();
        _keyboardHook.Dispose();
    }
}