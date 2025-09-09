
using System.Net.Http;
using System.Text.Json;

namespace ComputerLock.Components;

public partial class Donation
{
    [CascadingParameter]
    private IMudDialogInstance MudDialog { get; set; } = null!;
    private void Close() => MudDialog.Cancel();

    [Inject]
    private ISnackbar Snackbar { get; set; } = null!;
    [Inject]
    private IStringLocalizer<Lang> Lang { get; set; } = null!;

    [Inject]
    private ILogger Logger { get; set; } = null!;

    private int? _donationCount;
    private static readonly HttpClient HttpClient = new HttpClient();
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        try
        {
            var json = await HttpClient.GetStringAsync(Resource.DonationListPath);
            using JsonDocument doc = JsonDocument.Parse(json);
            if (doc.RootElement.ValueKind == JsonValueKind.Array)
            {
                _donationCount = doc.RootElement.GetArrayLength();
            }
        }
        catch (Exception ex)
        {
            Snackbar.Add(Lang["NetworkRequestFailed"], Severity.Error);
            Logger.Error(Lang["NetworkRequestFailed"], ex);
        }
    }
}