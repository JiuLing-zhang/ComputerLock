using ComputerLock.Update;

namespace ComputerLock.Components;

public partial class About
{
    private string _version = "";
    [Inject]
    private AppSettings AppSettings { get; set; } = null!;
    [Inject]
    private AppSettingsProvider AppSettingsProvider { get; set; } = null!;
    [Inject]
    private IStringLocalizer<Lang> Lang { get; set; } = null!;
    [Inject]
    private UpdateHelper UpdateHelper { get; set; } = null!;
    [Inject]
    private IDialogService DialogService { get; set; } = null!;

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        _version = $"v{AppBase.VersionString[..AppBase.VersionString.LastIndexOf('.')]}";
    }

    private async Task OpenVersionHistory()
    {
        var options = new DialogOptions
        {
            CloseButton = true,
            CloseOnEscapeKey = false,
            BackdropClick = false,
            BackgroundClass = "dialog-backdrop-filter",
            FullWidth = true
        };
        await DialogService.ShowAsync<VersionHistoryDialog>(Lang["VersionHistory"], options);
    }

    private async Task CheckUpdateAsync()
    {
        await UpdateHelper.DoAsync(false);
    }

    private void SaveSettings()
    {
        AppSettingsProvider.SaveSettings(AppSettings);
    }
}