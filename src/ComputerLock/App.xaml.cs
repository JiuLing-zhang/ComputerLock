﻿using System.Globalization;
using System.IO;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor.Services;
using System.Windows;
using ComputerLock.Update;
using Application = System.Windows.Application;

namespace ComputerLock;
/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    private static Mutex _mutex = default!;
    private WindowMain? _mainWindow;
    protected override void OnStartup(StartupEventArgs e)
    {
        _mutex = new Mutex(true, AppBase.FriendlyName);
        if (!_mutex.WaitOne(0, false))
        {
            System.Windows.MessageBox.Show("程序已经运行 The program has been run");
            Application.Current.Shutdown();
            return;
        }

        Environment.CurrentDirectory = Path.GetDirectoryName(AppBase.ExecutablePath);

        Init();
        base.OnStartup(e);
    }

    private void Init()
    {
        IServiceCollection services = new ServiceCollection();
        services.AddSingleton<AppSettingsProvider>();
        services.AddSingleton<AppSettings>((sp) =>
        {
            return sp.GetRequiredService<AppSettingsProvider>().LoadSettings();
        });
        services.AddSingleton(LogManager.GetLogger());
        services.AddSingleton<KeyboardHook>();
        services.AddSingleton<UpdateHelper>();
        services.AddSingleton<AutostartHook>();
        services.AddSingleton<TaskManagerHook>();
        services.AddSingleton<UserActivityMonitor>();
        services.AddSingleton<WindowMain>();
        services.AddTransient<WindowLockScreen>();
        services.AddTransient<WindowBlankScreen>();
        services.AddSingleton<LockService>();
        services.AddSingleton<IWindowMoving, WindowMoving>();
        services.AddSingleton<IWindowTitleBar, WindowTitleBar>();
        services.AddSingleton<ILocker, Locker>();
        services.AddLocalization();
        services.AddWpfBlazorWebView();
        services.AddBlazorWebViewDeveloperTools();
        services.AddMudServices(config =>
        {
            config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.TopLeft;
            config.SnackbarConfiguration.ShowCloseIcon = false;
            config.SnackbarConfiguration.VisibleStateDuration = 1500;
            config.SnackbarConfiguration.ShowTransitionDuration = 200;
            config.SnackbarConfiguration.HideTransitionDuration = 400;
        });

        var sp = services.BuildServiceProvider();
        Resources.Add("services", sp);

        var cultureInfo = new CultureInfo(sp.GetRequiredService<AppSettings>().Lang.ToString());
        CultureInfo.CurrentCulture = cultureInfo;
        Thread.CurrentThread.CurrentCulture = cultureInfo;
        Thread.CurrentThread.CurrentUICulture = cultureInfo;

        _mainWindow = sp.GetRequiredService<WindowMain>();
        Application.Current.MainWindow = _mainWindow;
        _mainWindow.Show();
    }

    protected override void OnExit(ExitEventArgs e)
    {
        _mainWindow?.Dispose();
        base.OnExit(e);
    }
}