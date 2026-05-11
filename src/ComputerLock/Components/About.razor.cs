using System.IO;
using Microsoft.JSInterop;

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
    private IDialogService DialogService { get; set; } = null!;

    [Inject]
    private IJSRuntime JS { get; set; } = null!;

    [Inject]
    private ISnackbar Snackbar { get; set; } = null!;


    private static readonly MudMarkdownStyling Styling = new()
    {
        Table =
        {
            IsBordered= false,
            Dense=true
        },
    };

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
}