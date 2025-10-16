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
    [JsonPropertyName("AppThemeInt")]
    public ThemeEnum AppTheme { get; set; } = ThemeEnum.System;

    /// <summary>
    /// 按下 ESC 键后最小化到托盘
    /// </summary>
    public bool IsHideWindowWhenEsc { get; set; } = false;

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
    /// Windows 锁定时自动解锁程序
    /// </summary>
    public bool IsUnlockWhenWindowsLock { get; set; } = true;

    /// <summary>
    /// 锁定时显示动画
    /// </summary>
    public bool LockAnimation { get; set; } = true;

    /// <summary>
    /// 锁定提示
    /// </summary>
    public bool LockTips { get; set; } = false;

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
    /// 解锁密码
    /// </summary>
    public string Password { get; set; } = "";

    /// <summary>
    /// 锁屏快捷键
    /// </summary>
    [JsonPropertyName("ShortcutKeyForLock")]
    public string LockHotkeyString { get; set; } = "";

    /// <summary>
    /// 锁屏快捷键(映射到按键名称，用于主界面显示)
    /// </summary>
    [JsonIgnore]
    public string LockHotkeyDisplay => _hotkeyTools?.StringKeyToDisplay(LockHotkeyString) ?? "";

    /// <summary>
    /// 锁屏快捷键
    /// </summary>
    [JsonIgnore]
    public Hotkey? LockHotkey => _hotkeyTools?.StringKeyToHotkey(LockHotkeyString);

    /// <summary>
    /// 解锁时使用锁屏快捷键
    /// </summary>
    public bool IsUnlockUseLockHotkey { get; set; } = true;

    /// <summary>
    /// 解锁快捷键
    /// </summary>
    public string UnlockHotkeyString { get; set; } = "";

    /// <summary>
    /// 解锁快捷键
    /// </summary>
    [JsonIgnore]
    public Hotkey? UnlockHotkey => _hotkeyTools?.StringKeyToHotkey(UnlockHotkeyString);

    /// <summary>
    /// 自动检查更新
    /// </summary>
    public bool IsAutoCheckUpdate { get; set; } = true;

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
    public ScreenUnlockMethods ScreenUnlockMethod { get; set; } = ScreenUnlockMethods.Hotkey;

    /// <summary>
    /// 锁屏状态展示
    /// </summary>
    public LockStatusDisplay LockStatusDisplay { get; set; } = LockStatusDisplay.None;

    /// <summary>
    /// 启用软件渲染
    /// </summary>
    public bool IsEnableSoftwareRendering { get; set; } = true;

    /// <summary>
    /// 锁定后执行关机/休眠（秒）
    /// </summary>
    public int AutoPowerSecond { get; set; } = 0;

    /// <summary>
    /// 锁定后执行关机/休眠（分钟）
    /// </summary>
    public int AutoPowerMinute => AutoPowerSecond / 60;

    /// <summary>
    /// 锁定后自动执行的电源操作类型
    /// </summary>
    public PowerActionType AutoPowerActionType { get; set; } = PowerActionType.Shutdown;

    /// <summary>
    /// 锁屏背景图片路径
    /// </summary>
    public string LockScreenBackgroundImage { get; set; } = "";
}