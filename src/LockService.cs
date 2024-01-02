using ComputerLock.Hooks;
using ComputerLock.Resources;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using System.Windows;
using System.Windows.Threading;

namespace ComputerLock;
internal class LockService
{
    private bool _isLocked = false;
    private readonly IServiceProvider _serviceProvider;
    private readonly SystemKeyHook _systemKeyHook = new();
    private WindowLockScreen _windowLockScreen;
    private readonly List<WindowBlankScreen> _blankScreens;
    private WindowPopup? _popup;
    private readonly IStringLocalizer<Lang> _lang;
    private readonly AppSettings _appSettings;
    public event EventHandler OnLock;
    public event EventHandler OnUnlock;
    public LockService(IServiceProvider serviceProvider, IStringLocalizer<Lang> lang, AppSettings appSettings)
    {
        _serviceProvider = serviceProvider;
        _blankScreens = new List<WindowBlankScreen>();
        _lang = lang;
        _appSettings = appSettings;
    }

    public void Lock()
    {
        if (_isLocked)
        {
            return;
        }
        var primaryScreen = Screen.PrimaryScreen;
        if (primaryScreen == null)
        {
            throw new Exception("没有检测到屏幕 no screen");
        }

        _isLocked = true;
        if (_appSettings.LockAnimation)
        {
            ShowPopup();
        }

        TaskManagerHook.DisabledTaskManager();
        _systemKeyHook.DisableSystemKey();
        if (_blankScreens.Any())
        {
            _blankScreens.Clear();
        }

        _windowLockScreen = _serviceProvider.GetRequiredService<WindowLockScreen>();
        _windowLockScreen.Left = primaryScreen.WorkingArea.Left;
        _windowLockScreen.Top = primaryScreen.WorkingArea.Top;
        _windowLockScreen.OnUnlock += _fmLockScreen_OnUnlock;
        _windowLockScreen.Closing += (_, __) =>
        {
            _windowLockScreen.OnUnlock -= _fmLockScreen_OnUnlock;
        };
        _windowLockScreen.Show();
        _windowLockScreen.Activate();

        for (var i = 0; i <= Screen.AllScreens.Length - 1; i++)
        {
            var screen = Screen.AllScreens[i];
            if (screen.Primary)
            {
                continue;
            }

            var blankScreen = _serviceProvider.GetRequiredService<WindowBlankScreen>();
            blankScreen.WindowStartupLocation = WindowStartupLocation.Manual;
            blankScreen.Left = screen.WorkingArea.Left;
            blankScreen.Top = screen.WorkingArea.Top;
            blankScreen.OnDeviceInput += BlankScreen_OnDeviceInput;
            blankScreen.Show();
            blankScreen.Activate();
            _blankScreens.Add(blankScreen);
        }

        OnLock?.Invoke(this, EventArgs.Empty);
    }

    private void _fmLockScreen_OnUnlock(object? sender, EventArgs e)
    {
        foreach (var screen in _blankScreens)
        {
            screen.OnDeviceInput -= BlankScreen_OnDeviceInput;
            screen.Unlock();
            screen.Close();
        }
        TaskManagerHook.EnabledTaskManager();
        _systemKeyHook.Dispose();
        _isLocked = false;
        OnUnlock?.Invoke(this, EventArgs.Empty);
    }
    private void BlankScreen_OnDeviceInput(object? sender, EventArgs e)
    {
        _windowLockScreen.ShowPassword();
    }

    private void ShowPopup()
    {
        _popup = new WindowPopup(_lang["Locked"]);
        double primaryScreenWidth = SystemParameters.PrimaryScreenWidth;
        double primaryScreenHeight = SystemParameters.PrimaryScreenHeight;
        _popup.Left = (primaryScreenWidth - _popup.Width) / 2;
        _popup.Top = (primaryScreenHeight - _popup.Height) / 2;
        _popup.Show();

        var timer = new DispatcherTimer()
        {
            Interval = TimeSpan.FromMilliseconds(1100),
        };
        timer.Tick += (_, __) =>
        {
            timer.Stop();
            _popup.CloseWindow();
        };
        timer.Start();
    }
}