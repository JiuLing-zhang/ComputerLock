namespace ComputerLock;
internal class AppConfigInfo
{
    /// <summary>
    /// 关闭窗口时最小化到托盘
    /// </summary>
    public bool IsHideWindowWhenClose { get; set; } = true;

    /// <summary>
    /// 启动后最小化到托盘
    /// </summary>
    public bool IsHideWindowWhenLaunch { get; set; } = true;

    /// <summary>
    /// 自动移动鼠标
    /// </summary>
    public bool IsDisableWindowsLock { get; set; } = true;

    /// <summary>
    /// 密码框的位置
    /// </summary>
    public ScreenLocationEnum PasswordInputLocation { get; set; } = ScreenLocationEnum.Center;

    /// <summary>
    /// 默认密码 1
    /// </summary>
    public string Password { get; set; } = "c4ca4238a0b923820dcc509a6f75849b";

    /// <summary>
    /// 初始密码已修改
    /// </summary>
    public bool IsPasswordChanged { get; set; } = false;

    /// <summary>
    /// 锁屏快捷键
    /// </summary>
    public string ShortcutKeyForLock { get; set; }
    /// <summary>
    /// 锁屏快捷键(映射到按键名称，用于主界面显示)
    /// </summary>
    public string ShortcutKeyDisplayForLock { get; set; }

    /// <summary>
    /// 自动检查更新
    /// </summary>
    public bool IsAutoCheckUpdate { get; set; } = false;

    /// <summary>
    /// 自动隐藏密码框
    /// </summary>
    public bool IsHidePasswordWindow { get; set; } = true;
}