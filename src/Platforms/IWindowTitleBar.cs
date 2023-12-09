namespace ComputerLock.Platforms;
public interface IWindowTitleBar
{
    public bool IsMaximized { get; }
    void Minimize();
    void Maximize();
    void Close();
}