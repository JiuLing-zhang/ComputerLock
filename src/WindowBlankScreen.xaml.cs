using ComputerLock.Enums;
using System.Windows;
using System.Windows.Input;

namespace ComputerLock
{
    /// <summary>
    /// WindowBlankScreen.xaml 的交互逻辑
    /// </summary>
    public partial class WindowBlankScreen : Window
    {
        private bool _isUnlock = false;
        public event EventHandler<EventArgs>? OnDeviceInput;
        private readonly double _unlockAreaHeight = 65;
        private readonly double _unlockAreaWidth = 230;

        private readonly AppSettings _appSettings;
        public WindowBlankScreen(AppSettings appSettings)
        {
            InitializeComponent();
            _appSettings = appSettings;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private void Window_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if ((_appSettings.PasswordBoxActiveMethod & PasswordBoxActiveMethodEnum.KeyboardDown) != PasswordBoxActiveMethodEnum.KeyboardDown)
            {
                return;
            }

            if (e.Key != Key.Escape)
            {
                return;
            }
            OnDeviceInput?.Invoke(this, EventArgs.Empty);
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //这里只响应左键
            //因为为了实现禁用 Windows 锁屏，会定时点击鼠标右键
            if (e.ChangedButton != MouseButton.Left)
            {
                return;
            }
            if ((_appSettings.PasswordBoxActiveMethod & PasswordBoxActiveMethodEnum.MouseDown) != PasswordBoxActiveMethodEnum.MouseDown)
            {
                return;
            }

            double xMin;
            double xMax;
            double yMin;
            double yMax;
            switch (_appSettings.PasswordInputLocation)
            {
                case ScreenLocationEnum.Center:
                    xMin = this.Width / 2 - _unlockAreaWidth / 2;
                    xMax = this.Width / 2 + _unlockAreaWidth / 2;
                    yMin = this.Height / 2 - _unlockAreaHeight / 2;
                    yMax = this.Height / 2 + _unlockAreaHeight / 2;
                    break;
                case ScreenLocationEnum.TopLeft:
                    xMin = 0;
                    xMax = _unlockAreaWidth;
                    yMin = 0;
                    yMax = _unlockAreaHeight;
                    break;
                case ScreenLocationEnum.TopRight:
                    xMin = this.Width - _unlockAreaWidth;
                    xMax = this.Width;
                    yMin = 0;
                    yMax = _unlockAreaHeight;
                    break;
                case ScreenLocationEnum.BottomLeft:
                    xMin = 0;
                    xMax = _unlockAreaWidth;
                    yMin = this.Height - _unlockAreaHeight;
                    yMax = this.Height;
                    break;
                case ScreenLocationEnum.BottomRight:
                    xMin = this.Width - _unlockAreaWidth;
                    xMax = this.Width;
                    yMin = this.Height - _unlockAreaHeight;
                    yMax = this.Height;
                    break;
                default:
                    xMin = this.Width / 2 - _unlockAreaWidth / 2;
                    xMax = this.Width / 2 + _unlockAreaWidth / 2;
                    yMin = this.Height / 2 - _unlockAreaHeight / 2;
                    yMax = this.Height / 2 + _unlockAreaHeight / 2;
                    break;
            }

            System.Windows.Point point = Mouse.GetPosition(e.Source as FrameworkElement);

            if (point.X >= xMin && point.X <= xMax && point.Y >= yMin && point.Y <= yMax)
            {
                OnDeviceInput?.Invoke(this, EventArgs.Empty);
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!_isUnlock)
            {
                e.Cancel = true;
            }
        }
        public void Unlock()
        {
            _isUnlock = true;
        }
    }
}
