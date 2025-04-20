
using System.Net.Http;
using System.Text.Json;

namespace ComputerLock.Components;

public partial class Donation
{
    [CascadingParameter]
    private IMudDialogInstance MudDialog { get; set; } = null!;
    private void Close() => MudDialog.Cancel();

    private int? _donationCount;
    private static readonly HttpClient HttpClient = new HttpClient();
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        var json = await HttpClient.GetStringAsync("https://www.jiuling.cc/api/donation/list");
        using JsonDocument doc = JsonDocument.Parse(json);
        if (doc.RootElement.ValueKind == JsonValueKind.Array)
        {
            _donationCount = doc.RootElement.GetArrayLength();
        }
    }
}