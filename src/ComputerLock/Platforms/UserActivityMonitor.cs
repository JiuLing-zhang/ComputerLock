using System.Runtime.InteropServices;
using System.Windows.Threading;
using Dispatcher = System.Windows.Threading.Dispatcher;

namespace ComputerLock.Platforms
{
    public class UserActivityMonitor : IDisposable
    {
        private readonly DispatcherTimer _timer;
        private int _autoLockMillisecond;
        private bool _isMonitoring;
        private readonly Dispatcher _dispatcher;
        private bool _disposed;
        // 记录开始监控的时间，用来延迟解锁后的检测空闲
        private long _lastStartTime;

        public event EventHandler? OnIdle;

        public UserActivityMonitor()
        {
            _dispatcher = Dispatcher.CurrentDispatcher;

            _timer = new DispatcherTimer(TimeSpan.FromSeconds(1), DispatcherPriority.Normal, Timer_Tick, _dispatcher);
            _timer.Start();
        }

        public void SetAutoLockSecond(int autoLockSecond)
        {
            RunOnUIThread(() =>
            {
                _autoLockMillisecond = autoLockSecond * 1000;
            });
        }

        public void StartMonitoring()
        {
            RunOnUIThread(() =>
            {
                _isMonitoring = true;
                _lastStartTime = Environment.TickCount64;
            });
        }

        public void StopMonitoring()
        {
            RunOnUIThread(() =>
            {
                _isMonitoring = false;
            });
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            if (!_isMonitoring || _disposed)
            {
                return;
            }

            // 检查是否已经监控了足够长的时间（至少2秒）
            long currentTime = Environment.TickCount64;
            long monitoringDuration = currentTime - _lastStartTime;
            if (monitoringDuration < 5000)
            {
                // 重新启动监控时，延迟 5 秒后再开始检测空闲
                return;
            }

            var lastInputInfo = new WinApi.LastInputInfo();
            lastInputInfo.cbSize = (uint)Marshal.SizeOf(lastInputInfo);

            if (WinApi.GetLastInputInfo(ref lastInputInfo))
            {
                long elapsedMillisecond = currentTime - lastInputInfo.dwTime;
                if (elapsedMillisecond > _autoLockMillisecond)
                {
                    OnIdle?.Invoke(this, EventArgs.Empty);
                    // 触发一次后停止监控
                    StopMonitoring();
                }
            }
        }

        private void RunOnUIThread(Action action)
        {
            if (_disposed)
            {
                return;
            }

            if (_dispatcher.CheckAccess())
            {
                action();
            }
            else
            {
                _dispatcher.Invoke(action);
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                RunOnUIThread(() =>
                {
                    _timer.Stop();
                    _timer.Tick -= Timer_Tick;
                });

                OnIdle = null;
            }
            _disposed = true;
        }
    }
}
