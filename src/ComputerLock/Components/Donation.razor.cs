namespace ComputerLock.Components;

public partial class Donation
{
    [CascadingParameter]
    private IMudDialogInstance MudDialog { get; set; } = null!;
    private void Close() => MudDialog.Cancel();
}