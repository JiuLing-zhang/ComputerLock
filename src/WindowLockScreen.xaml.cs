using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using Point = System.Windows.Point;

namespace ComputerLock;
/// <summary>
/// WindowLockScreen.xaml 的交互逻辑
/// </summary>
public partial class WindowLockScreen : Window
{
    private DateTime _hideSelfTime;

    private readonly int _hideSelfSecond = 3;
    private readonly DispatcherTimer _timer = new();
    private readonly AppSettings _appSettings;
    private readonly IStringLocalizer<Lang> _lang;
    private readonly ILogger _logger;

    public event EventHandler<EventArgs>? OnUnlock;

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
    private static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);
    public const int MOUSEEVENTF_LEFTDOWN = 0x0002;
    public const int MOUSEEVENTF_LEFTUP = 0x0004;
    public const int MOUSEEVENTF_RIGHTDOWN = 0x0008;
    public const int MOUSEEVENTF_RIGHTUP = 0x0010;

    public WindowLockScreen(AppSettings appSettings, IStringLocalizer<Lang> lang, ILogger logger)
    {
        InitializeComponent();
        _appSettings = appSettings;
        _lang = lang;
        _logger = logger;

        _timer.Interval = TimeSpan.FromSeconds(1);
        _timer.Tick += Timer_Tick;
        _timer.Start();
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        _logger.Write("主屏幕 -> 准备锁定");
        LblPassword.Content = _lang["Password"];

        _logger.Write($"启用密码框：{_appSettings.EnablePasswordBox}");
        if (_appSettings.EnablePasswordBox)
        {
            _logger.Write($"自动隐藏密码框：{_appSettings.IsHidePasswordWindow}");
            if (_appSettings.IsHidePasswordWindow)
            {
                LblMessage.Visibility = Visibility.Visible;
                LblMessage.Content = $"{_lang["TimerPrefix"]}{_hideSelfSecond}{_lang["TimerPostfix"]}";
            }
            RefreshHideSelfTime();
        }
        HidePassword();
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

    public void ShowPassword()
    {
        if (_appSettings.EnablePasswordBox)
        {
            _logger.Write("准备显示密码框");
            RefreshHideSelfTime();
            TxtPassword.Visibility = Visibility.Visible;
            PasswordBlock.Opacity = 1;
        }
        else
        {
            _logger.Write("准备无感解锁");
            TxtPassword.Visibility = Visibility.Visible;
            //TxtPassword.Width = 1;
            PasswordBlock.Width = 1;
            PasswordBlock.Opacity = 0.01;
        }
        //TODO 处理密码      
        TxtPassword.Password = "";
        TxtPassword.Focus();
    }

    private void Timer_Tick(object? sender, EventArgs e)
    {
        try
        {
            var time = DateTime.Now;
            if (time.Second % 30 == 0)
            {
                if (_appSettings.IsDisableWindowsLock)
                {
                    _logger.Write("移动鼠标，防止 Windows 锁屏");
                    DoMoveMouse();
                }
            }
            if (_appSettings.EnablePasswordBox)
            {
                if (_appSettings.IsHidePasswordWindow)
                {
                    var hideCountdown = Convert.ToInt32(_hideSelfTime.Subtract(time).TotalSeconds);
                    LblMessage.Content = $"{_lang["TimerPrefix"]}{hideCountdown}{_lang["TimerPostfix"]}";
                    if (hideCountdown <= 0)
                    {
                        _logger.Write("准备自动隐藏密码框");
                        HidePassword();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            _logger.Write($"定时组件异常。{ex.Message}。{ex.StackTrace}");
        }
    }

    private void TxtPassword_PasswordChanged(object sender, RoutedEventArgs e)
    {
        _logger.Write("检测到密码输入");
        if (_appSettings.EnablePasswordBox)
        {
            RefreshHideSelfTime();
        }
        var txt = TxtPassword.Password;
        if (txt.IsEmpty())
        {
            _logger.Write("输入为空，跳过");
            return;
        }
        if (_appSettings.Password != JiuLing.CommonLibs.Security.MD5Utils.GetStringValueToLower(txt))
        {
            return;
        }
        _logger.Write("密码正确，通知解锁");
        OnUnlock?.Invoke(this, EventArgs.Empty);
        this.Close();
    }

    private void TxtPassword_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
    {
        if (e.Key == Key.Enter || e.Key == Key.Escape)
        {
            _logger.Write("清空密码框");
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
    private void HidePassword()
    {
        _logger.Write("准备隐藏密码框");
        if (PasswordBlock.Opacity == 1)
        {
            TxtPassword.Visibility = Visibility.Collapsed;
            PasswordBlock.Opacity = 0;
        }
    }
    private void RefreshHideSelfTime()
    {
        _hideSelfTime = DateTime.Now.AddSeconds(_hideSelfSecond);
        this.Dispatcher.BeginInvoke(new Action(() =>
        {
            LblMessage.Content = $"{_lang["TimerPrefix"]}{_hideSelfSecond}{_lang["TimerPostfix"]}";
        }));
    }

    private void PasswordBlock_MouseDown(object sender, MouseButtonEventArgs e)
    {
        _logger.Write("密码框位置检测到点击");
        if (_appSettings.EnablePasswordBox)
        {
            if ((_appSettings.PasswordBoxActiveMethod & PasswordBoxActiveMethodEnum.MouseDown) != PasswordBoxActiveMethodEnum.MouseDown)
            {
                return;
            }
            _logger.Write("准备鼠标解锁密码框");
            ShowPassword();
        }
    }

    private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
        _logger.Write($"主屏幕 -> 准备关闭");
        _timer.Stop();
    }

    private void Window_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
    {
        _logger.Write("主屏幕 -> 检测到按键");
        if (e.Key != Key.Escape)
        {
            return;
        }
        _logger.Write("主屏幕 -> 按下ESC功能键");
        if (_appSettings.EnablePasswordBox)
        {
            if ((_appSettings.PasswordBoxActiveMethod & PasswordBoxActiveMethodEnum.KeyboardDown) != PasswordBoxActiveMethodEnum.KeyboardDown)
            {
                return;
            }
        }
        _logger.Write("主屏幕 -> 准备执行解锁");
        ShowPassword();
    }
}
