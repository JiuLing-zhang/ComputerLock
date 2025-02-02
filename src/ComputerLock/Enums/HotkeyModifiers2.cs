namespace ComputerLock.Enums;

/// <summary>
/// 热键修饰键
/// </summary>
[Flags]
public enum HotKeyModifiers : uint
{
    Alt = 1,
    Control = 2,
    Shift = 4,
    Win = 8
}
