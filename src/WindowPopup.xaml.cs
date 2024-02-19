using System.Windows;
using System.Windows.Media.Animation;

namespace ComputerLock
{
    /// <summary>
    /// WindowPopup.xaml 的交互逻辑
    /// </summary>
    public partial class WindowPopup : Window
    {
        public WindowPopup(string message)
        {
            InitializeComponent();
            TxtMessage.Text = message;
            Loaded += WindowPopup_Loaded;
        }
        private void WindowPopup_Loaded(object sender, RoutedEventArgs e)
        {
            // 在窗口加载完成后开始渐入动画
            var fadeInAnimation = new DoubleAnimation
            {
                From = 0,
                To = 1,
                Duration = TimeSpan.FromMilliseconds(300),
            };

            BeginAnimation(UIElement.OpacityProperty, fadeInAnimation);
        }

        public void CloseWindow()
        {
            // 在窗口关闭时开始渐出动画
            var fadeOutAnimation = new DoubleAnimation
            {
                From = 1,
                To = 0,
                Duration = TimeSpan.FromMilliseconds(300),
            };

            fadeOutAnimation.Completed += (_, _) => Close();

            BeginAnimation(UIElement.OpacityProperty, fadeOutAnimation);
        }
    }
}
