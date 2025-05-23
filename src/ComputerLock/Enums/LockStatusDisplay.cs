namespace ComputerLock.Enums;

/// <summary>
/// 锁定标识模式
/// </summary>
[Flags]
public enum LockStatusDisplay
{
    /// <summary>
    /// 不显示锁定标识
    /// </summary>
    None = 1,
    /// <summary>
    /// 顶部呼吸灯
    /// </summary>
    BreathingTop = 2,
    /// <summary>
    /// 左上角圆点
    /// </summary>
    DotTopLeft = 4,
    /// <summary>
    /// 右上角圆点
    /// </summary>
    DotTopRight = 8,
}