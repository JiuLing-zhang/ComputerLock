using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

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
        _logger.Info("功能屏幕 -> 准备锁定");
        LblPassword.Content = _lang["Password"];

        _logger.Info($"功能屏幕 -> 启用密码框：{_appSettings.EnablePasswordBox}");
        if (_appSettings.EnablePasswordBox)
        {
            _logger.Info($"功能屏幕 -> 自动隐藏密码框：{_appSettings.IsHidePasswordWindow}");
            if (_appSettings.IsHidePasswordWindow)
            {
                LblMessage.Visibility = Visibility.Visible;
                LblMessage.Content = string.Format(_lang["HideAfterXSecond"], _hideSelfSecond);
            }
            RefreshHideSelfTime();
        }
        HidePassword();

        if (_appSettings.LockStatusDisplay.HasFlag(LockStatusDisplay.BreathingTop))
        {
            _logger.Info("功能屏幕 -> 启用顶部呼吸灯");
            BreathingLightHelper.InitializeBreathingLight(TopBreathingLight);
        }
        if (_appSettings.LockStatusDisplay.HasFlag(LockStatusDisplay.DotTopLeft))
        {
            _logger.Info("功能屏幕 -> 启用左上角圆点");
            BreathingLightHelper.InitializeBreathingLight(DotTopLeft);
        }
        if (_appSettings.LockStatusDisplay.HasFlag(LockStatusDisplay.DotTopRight))
        {
            _logger.Info("功能屏幕 -> 启用右上角圆点");
            BreathingLightHelper.InitializeBreathingLight(DotTopRight);
        }
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
            _logger.Info("功能屏幕 -> 准备显示密码框");
            RefreshHideSelfTime();
            TxtPassword.Visibility = Visibility.Visible;
            PasswordBlock.Opacity = 1;
        }
        else
        {
            _logger.Info("功能屏幕 -> 准备无感解锁");
            TxtPassword.Visibility = Visibility.Visible;
            PasswordBlock.Width = 1;
            PasswordBlock.Opacity = 0.01;
        }
        TxtPassword.Password = "";
        TxtPassword.Focus();
    }

    private void Timer_Tick(object? sender, EventArgs e)
    {
        try
        {
            var time = DateTime.Now;
            if (_appSettings.EnablePasswordBox)
            {
                if (_appSettings.IsHidePasswordWindow)
                {
                    var hideCountdown = Convert.ToInt32(_hideSelfTime.Subtract(time).TotalSeconds);
                    LblMessage.Content = string.Format(_lang["HideAfterXSecond"], hideCountdown);
                    if (hideCountdown == 0)
                    {
                        _logger.Info("功能屏幕 -> 准备自动隐藏密码框");
                        HidePassword();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            _logger.Error($"功能屏幕 -> 定时组件异常", ex);
        }
    }

    private void TxtPassword_PasswordChanged(object sender, RoutedEventArgs e)
    {
        _logger.Info("功能屏幕 -> 检测到密码输入");
        if (_appSettings.EnablePasswordBox)
        {
            RefreshHideSelfTime();
        }
        var txt = TxtPassword.Password;
        if (txt.IsEmpty())
        {
            _logger.Info("功能屏幕 -> 输入为空，跳过");
            return;
        }
        if (_appSettings.Password != JiuLing.CommonLibs.Security.MD5Utils.GetStringValueToLower(txt))
        {
            return;
        }
        _logger.Info("功能屏幕 -> 密码正确，通知解锁");
        OnUnlock?.Invoke(this, EventArgs.Empty);
    }

    private void TxtPassword_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
    {
        if (e.Key == Key.Enter || e.Key == Key.Escape)
        {
            _logger.Info("功能屏幕 -> 清空密码框");
            TxtPassword.Password = "";
        }
    }

    private void HidePassword()
    {
        _logger.Info("功能屏幕 -> 准备隐藏密码框");
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
            LblMessage.Content = string.Format(_lang["HideAfterXSecond"], _hideSelfSecond);
        }));
    }

    private void PasswordBlock_MouseDown(object sender, MouseButtonEventArgs e)
    {
        _logger.Info("功能屏幕 -> 密码框位置检测到点击");
        if (_appSettings.EnablePasswordBox)
        {
            if ((_appSettings.PasswordBoxActiveMethod & PasswordBoxActiveMethodEnum.MouseDown) != PasswordBoxActiveMethodEnum.MouseDown)
            {
                return;
            }
            _logger.Info("功能屏幕 -> 准备鼠标解锁密码框");
            ShowPassword();
        }
    }

    private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
        _logger.Info($"功能屏幕 -> 准备关闭");
        _timer.Stop();
    }

    private void Window_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
    {
        _logger.Info("功能屏幕 -> 检测到按键");
        if (e.Key != Key.Escape)
        {
            return;
        }
        _logger.Info("功能屏幕 -> 按下ESC功能键");
        if (_appSettings.EnablePasswordBox)
        {
            if ((_appSettings.PasswordBoxActiveMethod & PasswordBoxActiveMethodEnum.KeyboardDown) != PasswordBoxActiveMethodEnum.KeyboardDown)
            {
                return;
            }
        }
        _logger.Info("功能屏幕 -> 准备执行解锁");
        ShowPassword();
    }
}
