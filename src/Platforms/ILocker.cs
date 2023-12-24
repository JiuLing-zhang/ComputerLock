namespace ComputerLock.Platforms;

public interface ILocker
{
    void Lock();
    event EventHandler OnLock;
    event EventHandler OnUnlock;
}