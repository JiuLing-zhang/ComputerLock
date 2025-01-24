namespace ComputerLock.Platforms;

/// <summary>
/// 锁定器接口，全局锁定：包括鼠标、键盘、任务管理器、系统快捷键等
/// </summary>
public interface ILocker : IDisposable
{
    void Lock();
}