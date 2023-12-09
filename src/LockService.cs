using ComputerLock.Hooks;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace ComputerLock;
internal class LockService
{
    private bool _isLocked = false;
    private readonly WindowLockScreen _fmLockScreen;
    private readonly IServiceProvider _serviceProvider;
    private readonly SystemKeyHook _systemKeyHook = new();
    private readonly List<WindowBlankScreen> _blankScreens;
    public LockService(WindowLockScreen windowLockScreen, IServiceProvider serviceProvider)
    {
        _fmLockScreen = windowLockScreen;
        _fmLockScreen.OnUnlock += _fmLockScreen_OnUnlock;
        SetFormLocation(_fmLockScreen, Screen.AllScreens[0]);
        _serviceProvider = serviceProvider;
        _blankScreens = new List<WindowBlankScreen>();
    }

    public void Lock()
    {
        if (_isLocked)
        {
            return;
        }
        _isLocked = true;
        TaskManagerHook.DisabledTaskManager();
        _systemKeyHook.DisableSystemKey();
        if (_blankScreens.Any())
        {
            _blankScreens.Clear();
        }

        for (var i = 0; i <= Screen.AllScreens.Length - 1; i++)
        {
            var blankScreen = _serviceProvider.GetRequiredService<WindowBlankScreen>();
            blankScreen.OnDeviceInput += BlankScreen_OnDeviceInput;
            SetFormLocation(blankScreen, Screen.AllScreens[i]);
            blankScreen.Show();
            blankScreen.Activate();
            _blankScreens.Add(blankScreen);
        }
        OpenLockScreen();
    }

    private void SetFormLocation(Window form, Screen screen)
    {
        //Rectangle bounds = screen.Bounds;
        ////TODO 迁移
        //form.Width = bounds.Width;
        //form.Height = bounds.Height;
        //form.SetBounds(bounds.X, bounds.Y, bounds.Width, bounds.Height);
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
        OpenLockScreen();
    }

    private void OpenLockScreen()
    {
        _fmLockScreen.Open();
    }
}