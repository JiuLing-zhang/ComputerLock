namespace ComputerLock.Platforms;

internal class Locker : ILocker
{
    private readonly LockService _lockService;
    public event EventHandler OnLock;
    public event EventHandler OnUnlock;

    public Locker(LockService lockService)
    {
        _lockService = lockService;
        _lockService.OnLock += (sender, e) => OnLock?.Invoke(sender, e);
        _lockService.OnUnlock += (sender, e) => OnUnlock?.Invoke(sender, e);
    }

    public void Lock()
    {
        _lockService.Lock();
    }
}