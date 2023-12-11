using ComputerLock.Hooks;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace ComputerLock;
internal class LockService
{
    private bool _isLocked = false;
    private readonly IServiceProvider _serviceProvider;
    private readonly SystemKeyHook _systemKeyHook = new();
    private WindowLockScreen _windowLockScreen;
    private readonly List<WindowBlankScreen> _blankScreens;
    public LockService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        _blankScreens = new List<WindowBlankScreen>();
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
    }
    private void BlankScreen_OnDeviceInput(object? sender, EventArgs e)
    {
        _windowLockScreen.ShowPassword();
    }
}