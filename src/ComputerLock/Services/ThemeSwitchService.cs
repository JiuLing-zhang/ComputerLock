namespace ComputerLock.Services;

/// <summary>
/// 主题切换服务
/// </summary>
public class ThemeSwitchService
{
    public ThemeEnum Theme { get; private set; }

    public event Action<ThemeEnum>? OnThemeChanged;

    public void SetDarkMode(ThemeEnum theme)
    {
        Theme = theme;
        OnThemeChanged?.Invoke(theme);
    }
}