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

            var lastInputInfo = new WinApi.LastInputInfo();
            lastInputInfo.cbSize = (uint)Marshal.SizeOf(lastInputInfo);

            if (WinApi.GetLastInputInfo(ref lastInputInfo))
            {
                long elapsedMillisecond = Environment.TickCount64 - lastInputInfo.dwTime;
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
