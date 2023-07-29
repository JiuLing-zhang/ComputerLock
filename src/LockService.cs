using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace ComputerLock;
internal class LockService
{
    private static readonly LockService Instance = new();
    public static LockService GetInstance()
    {
        return Instance;
    }

    private bool _isLocked = false;
    private readonly FmLockScreen _fmLockScreen;

    private readonly List<FmLockScreenBlank> _blankScreens;
    private LockService()
    {
        _fmLockScreen = new FmLockScreen();
        _fmLockScreen.OnUnlock += _fmLockScreen_OnUnlock;
        SetFormLocation(_fmLockScreen, Screen.AllScreens[0]);

        _blankScreens = new List<FmLockScreenBlank>();
    }

    public void Lock()
    {
        if (_isLocked)
        {
            return;
        }
        _isLocked = true;
        TaskManagerHook.DisabledTaskManager();

        if (_blankScreens.Any())
        {
            _blankScreens.Clear();
        }

        for (var i = 0; i <= Screen.AllScreens.Length - 1; i++)
        {
            var blankScreen = new FmLockScreenBlank();
            blankScreen.OnDeviceInput += BlankScreen_OnDeviceInput;
            SetFormLocation(blankScreen, Screen.AllScreens[i]);
            blankScreen.Show();
            blankScreen.Activate();
            _blankScreens.Add(blankScreen);
        }
        OpenLockScreen();
    }

    private void SetFormLocation(Form form, Screen screen)
    {
        Rectangle bounds = screen.Bounds;
        form.SetBounds(bounds.X, bounds.Y, bounds.Width, bounds.Height);
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