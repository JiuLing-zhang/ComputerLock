using JiuLing.TitleBarKit;

namespace ComputerLock.Components;

public partial class TitleBar
{
    [Parameter]
    public string Class { get; set; } = "";

    private string _maximizerIcon = default!;

    [Inject]
    private TitleBarService TitleBarService { get; set; } = default!;

    [Inject]
    private IStringLocalizer<Lang> Lang { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        await SetMaximizerIconAsync();
    }

    private Task MinimizeAsync()
    {
        TitleBarService.Controller.Minimize();
        return Task.CompletedTask;
    }

    private async Task MaximizeAsync()
    {
        TitleBarService.Controller.Maximize();
        await SetMaximizerIconAsync();
    }
    private Task CloseAsync()
    {
        TitleBarService.Controller.Close();
        return Task.CompletedTask;
    }

    private Task SetMaximizerIconAsync()
    {
        if (TitleBarService.Controller.IsMaximized)
        {
            _maximizerIcon = Icons.Material.Filled.CloseFullscreen;
        }
        else
        {
            _maximizerIcon = Icons.Material.Filled.Fullscreen;
        }
        return Task.CompletedTask;
    }
}