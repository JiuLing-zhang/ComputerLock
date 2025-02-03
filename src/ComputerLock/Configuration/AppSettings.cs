using System.Text.Json.Serialization;

namespace ComputerLock.Configuration;
public class AppSettings
{
    [JsonIgnore]
    private HotkeyTools? _hotkeyTools;

    public void Initialize(HotkeyTools hotkeyTools)
    {
        _hotkeyTools = hotkeyTools;
    }

    /// <summary>
    /// 主题
    /// </summary>
    public int AppThemeInt { get; set; } = 0;
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
    /// 锁定时显示动画
    /// </summary>
    public bool LockAnimation { get; set; } = true;

    /// <summary>
    /// 程序启动时锁定
    /// </summary>
    public bool LockOnStartup { get; set; } = false;

    /// <summary>
    /// 自动锁定的秒数
    /// </summary>
    public int AutoLockSecond { get; set; } = 0;

    /// <summary>
    /// 自动锁定的分钟数
    /// </summary>
    public int AutoLockMinute => AutoLockSecond / 60;

    /// <summary>
    /// 密码框的位置
    /// </summary>
    public ScreenLocationEnum PasswordInputLocation { get; set; } = ScreenLocationEnum.Center;

    /// <summary>
    /// 语言
    /// </summary>
    public LangEnum Lang { get; set; } = LangEnum.zh;

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
    public string ShortcutKeyForLock { get; set; } = "";

    /// <summary>
    /// 锁屏快捷键(映射到按键名称，用于主界面显示)
    /// </summary>
    [JsonIgnore]
    public string LockHotkeyDisplay => _hotkeyTools?.StringKeyToDisplay(ShortcutKeyForLock) ?? "";

    /// <summary>
    /// 锁屏快捷键
    /// </summary>
    [JsonIgnore]
    public Hotkey? LockHotkey => _hotkeyTools?.StringKeyToHotkey(ShortcutKeyForLock);


    /// <summary>
    /// 自动检查更新
    /// </summary>
    public bool IsAutoCheckUpdate { get; set; } = false;

    /// <summary>
    /// 启用密码框
    /// </summary>
    public bool EnablePasswordBox { get; set; } = true;

    /// <summary>
    /// 自动隐藏密码框
    /// </summary>
    public bool IsHidePasswordWindow { get; set; } = true;

    /// <summary>
    /// 密码框激活方式
    /// </summary>
    public PasswordBoxActiveMethodEnum PasswordBoxActiveMethod { get; set; } = PasswordBoxActiveMethodEnum.KeyboardDown | PasswordBoxActiveMethodEnum.MouseDown;

    /// <summary>
    /// 自动隐藏鼠标光标
    /// </summary>
    public bool IsHideMouseCursor { get; set; } = false;

    /// <summary>
    /// 屏幕解锁方式
    /// </summary>
    public ScreenUnlockMethods ScreenUnlockMethod { get; set; } = ScreenUnlockMethods.Password;
}