using System.Globalization;
using System.IO;
using ComputerLock.Platforms;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor;
using MudBlazor.Services;
using System.Windows;
using ComputerLock.Hooks;
using Application = System.Windows.Application;
using JiuLing.CommonLibs.ExtensionMethods;

namespace ComputerLock
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : System.Windows.Application
    {
        private static System.Threading.Mutex _mutex;
        protected override void OnStartup(StartupEventArgs e)
        {
            _mutex = new System.Threading.Mutex(true, AppBase.FriendlyName);
            if (!_mutex.WaitOne(0, false))
            {
                System.Windows.MessageBox.Show("程序已经运行 The program has been run");
                System.Windows.Application.Current.Shutdown();
                return;
            }

            Environment.CurrentDirectory = Path.GetDirectoryName(AppBase.ExecutablePath);

            Init();
            base.OnStartup(e);
        }

        private void Init()
        {
            IServiceCollection services = new ServiceCollection();
            services.AddSingleton<AppSettingWriter>();
            services.AddSingleton<AppSettings>((sp) =>
            {
                if (!File.Exists(AppBase.ConfigPath))
                {
                    return new AppSettings();
                }
                string json = File.ReadAllText(AppBase.ConfigPath);
                return System.Text.Json.JsonSerializer.Deserialize<AppSettings>(json);
            });

            services.AddSingleton<KeyboardHook>();
            services.AddSingleton<UpdateHelper>();
            services.AddSingleton<AutostartHook>();
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

            var mainWindow = sp.GetRequiredService<WindowMain>();
            Application.Current.MainWindow = mainWindow;
            mainWindow.Show();
        }
    }
}
