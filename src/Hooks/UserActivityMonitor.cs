using System.Runtime.InteropServices;
using System.Timers;
using Timer = System.Timers.Timer;

namespace ComputerLock.Hooks;

public class UserActivityMonitor : IDisposable
{

    [DllImport("user32.dll")]
    private static extern bool GetLastInputInfo(ref LastInputInfo plii);
    struct LastInputInfo
    {
        public uint cbSize;
        public uint dwTime;
    }

    private Timer? _timer;
    public EventHandler? OnIdle;

    private int _autoLockMillisecond;
    private bool _isMonitoring = false;
    public void Init(int autoLockSecond)
    {
        _autoLockMillisecond = autoLockSecond * 1000;

        _timer = new Timer();
        _timer.Interval = 1000;
        _timer.Elapsed += Timer_Elapsed;
        _timer.Start();
    }

    public void StartMonitoring()
    {
        _isMonitoring = true;
    }

    public void StopMonitoring()
    {
        _isMonitoring = false;
    }
    private void Timer_Elapsed(object? sender, ElapsedEventArgs e)
    {
        if (!_isMonitoring)
        {
            return;
        }

        var lastInputInfo = new LastInputInfo();
        lastInputInfo.cbSize = (uint)Marshal.SizeOf(lastInputInfo);

        if (GetLastInputInfo(ref lastInputInfo))
        {
            long elapsedMillisecond = (long)Environment.TickCount64 - lastInputInfo.dwTime;
            if (elapsedMillisecond > _autoLockMillisecond)
            {
                OnIdle?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    public void Dispose()
    {
        _timer?.Stop();
        _timer?.Dispose();
    }
}