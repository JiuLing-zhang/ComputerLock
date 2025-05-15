using System.Runtime.InteropServices;
using System.Timers;
using Timer = System.Timers.Timer;

namespace ComputerLock.Platforms;

public class UserActivityMonitor
{
    private Timer? _timer;
    public EventHandler? OnIdle;
    private int _autoLockMillisecond;
    private readonly object _lock = new();

    public void SetAutoLockSecond(int autoLockSecond)
    {
        _autoLockMillisecond = autoLockSecond * 1000;
    }

    public void StartMonitoring()
    {
        lock (_lock)
        {
            if (_timer == null)
            {
                _timer = new Timer();
                _timer.Interval = 1000;
                _timer.Elapsed += Timer_Elapsed;
            }
            _timer.Start();
        }
    }

    public void StopMonitoring()
    {
        lock (_lock)
        {
            if (_timer != null)
            {
                _timer.Elapsed -= Timer_Elapsed;
                _timer.Stop();
                _timer.Dispose();
                _timer = null;
            }
        }
    }

    private void Timer_Elapsed(object? sender, ElapsedEventArgs e)
    {
        var lastInputInfo = new WinApi.LastInputInfo();
        lastInputInfo.cbSize = (uint)Marshal.SizeOf(lastInputInfo);

        if (WinApi.GetLastInputInfo(ref lastInputInfo))
        {
            long elapsedMillisecond = (long)Environment.TickCount64 - lastInputInfo.dwTime;
            if (elapsedMillisecond > _autoLockMillisecond)
            {
                OnIdle?.Invoke(this, EventArgs.Empty);
                StopMonitoring();
            }
        }
    }
}