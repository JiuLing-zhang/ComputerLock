using ComputerLock.Interfaces;
using ComputerLock.Update;

namespace ComputerLock.Shared;
public partial class MainLayout
{
    private bool _isDarkMode;
    private MudThemeProvider _mudThemeProvider = null!;
    private MudTheme _customTheme = null!;

    [Inject]
    private IWindowMoving WindowMoving { get; set; } = null!;

    [Inject]
    private AppSettings AppSettings { get; set; } = null!;

    [Inject]
    private AppSettingsProvider AppSettingsProvider { get; set; } = null!;

    [Inject]
    private IStringLocalizer<Lang> Lang { get; set; } = null!;

    [Inject]
    private IDialogService Dialog { get; set; } = null!;

    [Inject]
    private VersionLogChecker VersionLogChecker { get; set; } = null!;


    [Inject]
    private UpdateHelper UpdateHelper { get; set; } = null!;

    [Inject]
    private IGlobalLockService GlobalLockService { get; set; } = null!;

    [Inject]
    private HotkeyHook HotkeyHook { get; set; } = null!;

    [Inject]
    private ILogger Logger { get; set; } = null!;

    [Inject]
    private ISnackbar Snackbar { get; set; } = null!;

    [Inject]
    private ThemeSwitchService ThemeSwitchService { get; set; } = null!;

    private static bool _isInitialized = false;
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        _customTheme = new MudTheme()
        {
            LayoutProperties = new LayoutProperties
            {
                AppbarHeight = "32px",
                DrawerWidthLeft = "200px"
            },

            PaletteLight = new PaletteLight()
            {
                Primary = "#fb8c00",
                Secondary = "#FFD54F",
                AppbarBackground = "#F5F5F5",
                AppbarText = "#fb8c00",
                Background = "#FFFFFF",
                Surface = "#FFFFFF",
                TableStriped = "#FAFAFA",
                TableHover = "#E3F2FD",
                LinesDefault = "#E0E0E0",

                Success = "#43A047",
                Error = "#E53935",
                Warning = "#FDD835",
                Info = "#29B6F6",
                TextPrimary = "#212121",
                TextSecondary = "#616161",
                DrawerBackground = "#FFFFFF",
                DrawerText = "#212121",
                ActionDefault = "#9E9E9E"
            },

            PaletteDark = new PaletteDark()
            {
                Primary = "#fb8c00",
                Secondary = "#FFA726",
                AppbarBackground = "#212121",
                AppbarText = "#fb8c00",
                Background = "#333333",
                Surface = "#424242",
                TableStriped = "#292929",
                TableHover = "#37474F",
                LinesDefault = "#555555",

                Success = "#66BB6A",
                Error = "#EF5350",
                Warning = "#FFEE58",
                Info = "#4FC3F7",
                TextPrimary = "#FFFFFF",
                TextSecondary = "#B0BEC5",
                DrawerBackground = "#2C2C2C",
                DrawerText = "#FFFFFF",
                ActionDefault = "#BDBDBD"
            }
        };

        if (AppSettings.IsAutoCheckUpdate)
        {
            await UpdateHelper.DoAsync(true);
        }

        if (await VersionLogChecker.CheckShowUpdateLogAsync())
        {
            await Dialog.ShowAsync<VersionHistoryDialog>("");
        }

        await SwitchThemeAsync(AppSettings.AppThemeInt);

        InitializeEventBinding();
    }

    private async Task SwitchThemeAsync(ThemeEnum theme)
    {
        _isDarkMode = theme switch
        {
            ThemeEnum.System => await _mudThemeProvider.GetSystemPreference(),
            ThemeEnum.Light => false,
            ThemeEnum.Dark => true,
            _ => _isDarkMode
        };
        await InvokeAsync(StateHasChanged);
    }

    private void InitializeEventBinding()
    {
        if (_isInitialized)
        {
            return;
        }
        _isInitialized = true;

        if (AppSettings.LockHotkeyString.IsNotEmpty())
        {
            RegisterLockHotkey();
        }
        if (AppSettings.UnlockHotkeyString.IsNotEmpty())
        {
            RegisterUnlockHotkey();
        }

        HotkeyHook.HotkeyPressed += (id) =>
        {
            if (id == (int)HotkeyType.Lock)
            {
                if (!GlobalLockService.IsLocked)
                {
                    Logger.Write("快捷键锁定");
                    GlobalLockService.Lock();
                }
                else
                {
                    if (AppSettings.ScreenUnlockMethod == ScreenUnlockMethods.Hotkey && AppSettings.IsUnlockUseLockHotkey)
                    {
                        Logger.Write("快捷键解锁");
                        GlobalLockService.Unlock();
                    }
                }
            }
            else if (id == (int)HotkeyType.Unlock)
            {
                if (GlobalLockService.IsLocked)
                {
                    Logger.Write("快捷键解锁（独立解锁）");
                    GlobalLockService.Unlock();
                }
            }
        };

        ThemeSwitchService.OnThemeChanged += async theme =>
        {
            await SwitchThemeAsync(theme);
        };
    }

    private void MouseDown()
    {
        WindowMoving.MouseDown();
    }

    private void MouseUp()
    {
        WindowMoving.MouseUp();
    }
    private async Task OpenDonationDialog()
    {
        var noHeader = new DialogOptions()
        {
            NoHeader = true,
            BackgroundClass = "dialog-blurry",
            CloseOnEscapeKey = false,
            FullWidth = true
        };
        await Dialog.ShowAsync<Donation>("", noHeader);
    }

    public void RegisterLockHotkey()
    {
        try
        {
            if (AppSettings.LockHotkey != null)
            {
                Logger.Write("注册锁屏热键");
                HotkeyHook.Register((int)HotkeyType.Lock, AppSettings.LockHotkey);
            }
        }
        catch (Exception ex)
        {
            Logger.Write($"绑定锁屏热键失败：{ex.Message}。{ex.StackTrace}");
            Snackbar.Add($"{Lang["ExRegistFailed"]}{ex.Message}", Severity.Error);
        }
    }

    public void RegisterUnlockHotkey()
    {
        try
        {
            if (AppSettings.UnlockHotkey != null)
            {
                Logger.Write("注册解锁热键");
                HotkeyHook.Register((int)HotkeyType.Unlock, AppSettings.UnlockHotkey);
            }
        }
        catch (Exception ex)
        {
            Logger.Write($"绑定解锁热键失败：{ex.Message}。{ex.StackTrace}");
            Snackbar.Add($"{Lang["ExRegistFailed"]}{ex.Message}", Severity.Error);
        }
    }
}