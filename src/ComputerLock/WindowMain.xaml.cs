﻿using System.IO;
using System.Windows;
using ComputerLock.Interfaces;

namespace ComputerLock;
public partial class WindowMain : Window, IDisposable
{
    private readonly AppSettings _appSettings;
    private readonly IGlobalLockService _globalLockService;
    private readonly ILogger _logger;

    private readonly NotifyIcon _notifyIcon = new();
    private readonly ContextMenuStrip _contextMenuStrip = new();

    public WindowMain(AppSettings appSettings, IGlobalLockService globalLockService, ILogger logger)
    {
        InitializeComponent();

        _appSettings = appSettings;
        _globalLockService = globalLockService;
        _logger = logger;

        InitializeNotifyIcon();
        _logger.Info("系统启动");

        if (_appSettings.LockOnStartup)
        {
            _logger.Info("程序启动时锁定屏幕");
            _globalLockService.Lock();
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
            _logger.Info("托盘锁定");
            _globalLockService.Lock();
        };
        _contextMenuStrip.Items.Add(btnLock);

        var btnClose = new ToolStripMenuItem(Lang.Exit);
        btnClose.Click += (_, _) =>
        {
            _logger.Info("托盘关闭");
            System.Windows.Application.Current.Shutdown();
        };
        _contextMenuStrip.Items.Add(btnClose);

        _notifyIcon.ContextMenuStrip = _contextMenuStrip;
        Stream iconStream = System.Windows.Application.GetResourceStream(new Uri("pack://application:,,,/ComputerLock;component/icon.ico")).Stream;
        _notifyIcon.Icon = new Icon(iconStream);
        _notifyIcon.Text = Lang.Title;
        _notifyIcon.Click += (_, e) =>
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
        if (_appSettings.IsHideWindowWhenEsc)
        {
            if (e.Key == System.Windows.Input.Key.Escape)
            {
                this.WindowState = WindowState.Minimized;
                this.ShowInTaskbar = false;
            }
        }
    }
    public void Dispose()
    {
        _logger.Info("系统资源释放，系统关闭");
        _notifyIcon.Dispose();
        _globalLockService.Dispose();
    }
}