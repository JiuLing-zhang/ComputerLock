namespace ComputerLock.Components;
public partial class TitleBar
{
    [Parameter]
    public string Class { get; set; } = "";

    private string _maximizerIcon = default!;

    [Inject]
    private IWindowTitleBar WindowTitleBar { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        await SetMaximizerIconAsync();
    }

    private Task MinimizeAsync()
    {
        WindowTitleBar.Minimize();
        return Task.CompletedTask;
    }

    private async Task MaximizeAsync()
    {
        WindowTitleBar.Maximize();
        await SetMaximizerIconAsync();
    }
    private Task CloseAsync()
    {
        WindowTitleBar.Close();
        return Task.CompletedTask;
    }

    private Task SetMaximizerIconAsync()
    {
        if (WindowTitleBar.IsMaximized)
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