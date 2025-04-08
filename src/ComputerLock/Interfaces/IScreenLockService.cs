namespace ComputerLock.Interfaces;

/// <summary>
/// 屏幕锁定接口，负责屏幕窗口的锁定和解锁
/// </summary>
internal interface IScreenLockService
{
    event EventHandler? OnUnlock;
    bool Lock(bool showAnimation);
    void Unlock();
}