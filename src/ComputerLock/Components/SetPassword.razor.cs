namespace ComputerLock.Components;
public partial class SetPassword
{
    [Inject]
    private IStringLocalizer<Lang> Lang { get; set; } = default!;

    [Inject]
    private ISnackbar Snackbar { get; set; } = default!;

    private string _password = "";

    [Parameter]
    public EventCallback<string> PasswordSetFinished { get; set; }

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
    }

    private async Task SavePasswordAsync()
    {
        if (_password.IsEmpty())
        {
            Snackbar.Add(Lang["PasswordEmpty"], Severity.Error);
            return;
        }

        var password = JiuLing.CommonLibs.Security.MD5Utils.GetStringValueToLower(_password);
        await PasswordSetFinished.InvokeAsync(password);
    }
}