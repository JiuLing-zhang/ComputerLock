using System.ComponentModel;

namespace ComputerLock.Enums;

/// <summary>
/// 屏幕解锁方法
/// </summary>
public enum ScreenUnlockMethods
{
    [Description("快捷键解锁")]
    Hotkey = 1,
    [Description("密码解锁")]
    Password = 2
}