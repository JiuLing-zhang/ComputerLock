namespace ComputerLock.Platforms;
internal class Locker : ILocker
{
    private readonly LockService _lockService;
    public Locker(LockService lockService)
    {
        _lockService = lockService;
    }
    public void Lock()
    {
        _lockService.Lock();
    }
}