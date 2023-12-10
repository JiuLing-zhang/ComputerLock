﻿using ComputerLock.Resources;
using JiuLing.CommonLibs.ExtensionMethods;
using Microsoft.Extensions.Localization;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using ComputerLock.Enums;
using Point = System.Windows.Point;

namespace ComputerLock;
/// <summary>
/// WindowLockScreen.xaml 的交互逻辑
/// </summary>
public partial class WindowLockScreen : Window
{
    private bool _isLocked;
    private DateTime _hideSelfTime;
    private int _hideSelfSecond = 3;
    public event EventHandler<EventArgs>? OnUnlock;
    private DispatcherTimer _timer = new DispatcherTimer();
    private readonly AppSettings _appSettings;
    private readonly IStringLocalizer<Lang> _lang;

    /// <summary>
    /// 引用user32.dll动态链接库（windows api），
    /// 使用库中定义 API：SetCursorPos 
    /// </summary>
    [DllImport("user32.dll")]
    private static extern int SetCursorPos(int x, int y);
    /// <summary>
    /// 移动鼠标到指定的坐标点
    /// </summary>
    public void MoveMouseToPoint(Point p)
    {
        SetCursorPos((int)p.X, (int)p.Y);
    }

    //点击事件
    [DllImport("User32")]
    public static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);
    public const int MOUSEEVENTF_LEFTDOWN = 0x0002;
    public const int MOUSEEVENTF_LEFTUP = 0x0004;
    public const int MOUSEEVENTF_RIGHTDOWN = 0x0008;
    public const int MOUSEEVENTF_RIGHTUP = 0x0010;

    public WindowLockScreen(AppSettings appSettings, IStringLocalizer<Lang> lang)
    {
        InitializeComponent();
        _appSettings = appSettings;
        _lang = lang;


        _timer.Interval = TimeSpan.FromSeconds(1);
        _timer.Tick += _timer_Tick;
        _timer.Start();
    }


    private void Window_Loaded(object sender, RoutedEventArgs e)
    {

    }

    private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
    {
        switch (_appSettings.PasswordInputLocation)
        {
            case ScreenLocationEnum.Center:
                PasswordBlock.VerticalAlignment = VerticalAlignment.Center;
                PasswordBlock.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                break;
            case ScreenLocationEnum.TopLeft:
                PasswordBlock.VerticalAlignment = VerticalAlignment.Top;
                PasswordBlock.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                break;
            case ScreenLocationEnum.TopRight:
                PasswordBlock.VerticalAlignment = VerticalAlignment.Top;
                PasswordBlock.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
                break;
            case ScreenLocationEnum.BottomLeft:
                PasswordBlock.VerticalAlignment = VerticalAlignment.Bottom;
                PasswordBlock.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                break;
            case ScreenLocationEnum.BottomRight:
                PasswordBlock.VerticalAlignment = VerticalAlignment.Bottom;
                PasswordBlock.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
                break;
            default:
                PasswordBlock.VerticalAlignment = VerticalAlignment.Center;
                PasswordBlock.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                break;
        }
    }

    public void Open()
    {
        if (this.Visibility == Visibility.Visible)
        {
            return;
        }
        _isLocked = true;
        LblPassword.Content = _lang["Password"];
        if (_appSettings.IsHidePasswordWindow)
        {
            LblMessage.Visibility = Visibility.Visible;
            LblMessage.Content = $"{_lang["TimerPrefix"]}{_hideSelfSecond}{_lang["TimerPostfix"]}";
        }
        RefreshHideSelfTime();
        this.Show();
        this.Topmost = true;
        this.Activate();
        TxtPassword.Password = "";
        TxtPassword.Focus();
    }


    private void _timer_Tick(object? sender, EventArgs e)
    {
        try
        {
            if (!_isLocked)
            {
                return;
            }
            var time = DateTime.Now;
            if (time.Second % 30 == 0)
            {
                if (_appSettings.IsDisableWindowsLock)
                {
                    DoMoveMouse();
                }
            }

            if (_appSettings.IsHidePasswordWindow)
            {
                var hideCountdown = Convert.ToInt32(_hideSelfTime.Subtract(time).TotalSeconds);
                LblMessage.Content = $"{_lang["TimerPrefix"]}{hideCountdown}{_lang["TimerPostfix"]}";
                if (hideCountdown <= 0)
                {
                    HideSelf();
                }
            }

        }
        catch (Exception ex)
        {
        }
    }

    private void TxtPassword_PasswordChanged(object sender, RoutedEventArgs e)
    {
        RefreshHideSelfTime();
        var txt = TxtPassword.Password;
        if (txt.IsEmpty())
        {
            return;
        }

        if (_appSettings.Password != JiuLing.CommonLibs.Security.MD5Utils.GetStringValueToLower(txt))
        {
            return;
        }

        HideSelf();
        _isLocked = false;
        OnUnlock?.Invoke(this, EventArgs.Empty);
    }

    private void TxtPassword_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {
            TxtPassword.Password = "";
        }
    }
    private void DoMoveMouse()
    {
        var random = new Random();
        var x = random.Next(0, 100);
        var y = random.Next(0, 100);

        MoveMouseToPoint(new Point(x, y));
        mouse_event(MOUSEEVENTF_RIGHTDOWN | MOUSEEVENTF_RIGHTUP, x, y, 0, 0);
    }
    private void HideSelf()
    {
        if (this.Visibility == Visibility.Visible)
        {
            this.Hide();
        }
    }

    private void RefreshHideSelfTime()
    {
        _hideSelfTime = DateTime.Now.AddSeconds(_hideSelfSecond);
    }

}
