﻿using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Timers;
using Timer = System.Timers.Timer;

namespace ComputerLock.Hooks;

public class UserActivityMonitor
{
    private readonly Stopwatch _activityStopwatch;
    private readonly Timer _idleTimer;

    private IntPtr _keyboardHook;
    private IntPtr _mouseHook;

    private readonly HookProc _keyboardHookCallback;
    private readonly HookProc _mouseHookCallback;

    private const int WH_KEYBOARD_LL = 13;
    private const int WH_MOUSE_LL = 14;
    public EventHandler OnIdle;

    private int _autoLockSecond;
    public UserActivityMonitor()
    {
        _activityStopwatch = new Stopwatch();
        _activityStopwatch.Start();

        _idleTimer = new Timer();
        _idleTimer.Interval = 1000;
        _idleTimer.Elapsed += IdleTimer_Tick;

        // 初始化钩子回调函数
        _keyboardHookCallback = KeyboardHookCallback;
        _mouseHookCallback = MouseHookCallback;
    }

    public void SetAutoLockSecond(int autoLockSecond)
    {
        _autoLockSecond = autoLockSecond;
    }

    public void StartMonitoring()
    {
        // 启动键盘和鼠标钩子
        _keyboardHook = SetWindowsHookEx(WH_KEYBOARD_LL, _keyboardHookCallback, GetModuleHandle(Process.GetCurrentProcess().MainModule.ModuleName), 0);
        _mouseHook = SetWindowsHookEx(WH_MOUSE_LL, _mouseHookCallback, GetModuleHandle(Process.GetCurrentProcess().MainModule.ModuleName), 0);

        _idleTimer.Start();
    }

    public void StopMonitoring()
    {
        // 停止键盘和鼠标钩子
        UnhookWindowsHookEx(_keyboardHook);
        UnhookWindowsHookEx(_mouseHook);

        _idleTimer.Stop();
        _activityStopwatch.Restart();
    }

    private void IdleTimer_Tick(object sender, ElapsedEventArgs e)
    {
        TimeSpan idleTime = _activityStopwatch.Elapsed;
        if (idleTime.TotalSeconds > _autoLockSecond)
        {
            OnIdle?.Invoke(this, EventArgs.Empty);
        }
    }

    private IntPtr KeyboardHookCallback(int nCode, IntPtr wParam, IntPtr lParam)
    {
        if (nCode >= 0 && (wParam == (IntPtr)WM_KEYDOWN || wParam == (IntPtr)WM_SYSKEYDOWN))
        {
            _activityStopwatch.Restart();
        }

        return CallNextHookEx(_keyboardHook, nCode, wParam, lParam);
    }

    private IntPtr MouseHookCallback(int nCode, IntPtr wParam, IntPtr lParam)
    {
        if (wParam == (IntPtr)WM_LBUTTONDOWN || wParam == (IntPtr)WM_RBUTTONDOWN || wParam == (IntPtr)WM_MBUTTONDOWN || wParam == (IntPtr)WM_XBUTTONDOWN)
        {
            _activityStopwatch.Restart();
        }
        else if (wParam == (IntPtr)WM_MOUSEMOVE)
        {
            // 处理鼠标移动事件，重置计时器
            _activityStopwatch.Restart();
        }

        return CallNextHookEx(_mouseHook, nCode, wParam, lParam);
    }

    // 钩子相关的声明和函数

    [StructLayout(LayoutKind.Sequential)]
    private struct KBDLLHOOKSTRUCT
    {
        public Keys key;
        public int scanCode;
        public int flags;
        public int time;
        public IntPtr dwExtraInfo;
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct MSLLHOOKSTRUCT
    {
        public Point pt;
        public int mouseData;
        public int flags;
        public int time;
        public IntPtr dwExtraInfo;
    }

    private delegate IntPtr HookProc(int nCode, IntPtr wParam, IntPtr lParam);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr SetWindowsHookEx(int idHook, HookProc lpfn, IntPtr hMod, uint dwThreadId);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool UnhookWindowsHookEx(IntPtr hhk);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

    [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr GetModuleHandle(string lpModuleName);

    private const int WM_KEYDOWN = 0x0100;
    private const int WM_SYSKEYDOWN = 0x0104;
    private const int WM_MOUSEMOVE = 0x0200;
    private const int WM_LBUTTONDOWN = 0x0201;
    private const int WM_RBUTTONDOWN = 0x0204;
    private const int WM_MBUTTONDOWN = 0x0207;
    private const int WM_XBUTTONDOWN = 0x020B;

    // 定义其他结构和函数

    [StructLayout(LayoutKind.Sequential)]
    private struct Point
    {
        public int X;
        public int Y;
    }

    private const uint INPUT_KEYBOARD = 1;
    private const uint KEYEVENTF_KEYUP = 0x0002;

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool GetCursorPos(out Point lpPoint);

    [DllImport("user32.dll")]
    private static extern bool ScreenToClient(IntPtr hWnd, ref Point lpPoint);
}