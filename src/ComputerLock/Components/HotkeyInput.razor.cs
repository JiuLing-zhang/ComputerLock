namespace ComputerLock.Components;

public partial class HotkeyInput
{
    private string _lockHotkeyDisplay => HotkeyTools.StringKeyToDisplay(Hotkey) ?? "";

    [Parameter]
    public string Title { get; set; } = null!;

    [Parameter]
    public bool Disabled { get; set; } = false;

    [Parameter]
    public string Hotkey { get; set; } = null!;

    [Parameter]
    public EventCallback<string> OnHotkeySet { get; set; }

    [Parameter]
    public EventCallback OnHotkeyClear { get; set; }


    [Inject]
    private IStringLocalizer<Lang> Lang { get; set; } = null!;

    [Inject]
    private IDialogService Dialog { get; set; } = null!;

    [Inject]
    private HotkeyTools HotkeyTools { get; set; } = null!;


    private async Task SetShortcutKey()
    {
        var options = new DialogOptions { NoHeader = true, CloseOnEscapeKey = false, BackdropClick = false, BackgroundClass = "dialog-backdrop-filter" };
        var dialog = await Dialog.ShowAsync<HotkeySetting>("", options);
        var result = await dialog.Result;
        if (result!.Canceled)
        {
            return;
        }

        await OnHotkeySet.InvokeAsync(result.Data!.ToString());
    }

    private async Task ClearShortcutKey()
    {
        await OnHotkeyClear.InvokeAsync();
    }
}
