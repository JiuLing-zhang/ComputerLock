using System.Windows;
using System.Windows.Threading;
using Application = System.Windows.Application;

namespace ComputerLock.Services;

/// <summary>
/// 弹窗服务，用于显示各种提示信息
/// </summary>
public class PopupService(ILogger logger)
{
    /// <summary>
    /// 显示提示信息
    /// </summary>
    /// <param name="message">提示信息</param>
    /// <param name="duration">显示时长（毫秒）</param>
    public void ShowMessage(string message, int duration = 1100)
    {
        Application.Current.Dispatcher.Invoke(() =>
        {
            logger.Write($"弹窗服务 -> 显示消息：{message}");
            var popup = new WindowPopup(message);
            double primaryScreenWidth = SystemParameters.PrimaryScreenWidth;
            double primaryScreenHeight = SystemParameters.PrimaryScreenHeight;
            popup.Left = (primaryScreenWidth - popup.Width) / 2;
            popup.Top = (primaryScreenHeight - popup.Height) / 2;
            popup.Show();

            var timer = new DispatcherTimer()
            {
                Interval = TimeSpan.FromMilliseconds(duration),
            };
            timer.Tick += (_, _) =>
            {
                timer.Stop();
                popup.CloseWindow();
            };
            timer.Start();
        });
    }
}