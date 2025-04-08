using DialogResult = MudBlazor.DialogResult;

namespace ComputerLock.Components;
public partial class SetPassword
{
    [CascadingParameter]
    private IMudDialogInstance MudDialog { get; set; } = default!;

    [Inject]
    private IStringLocalizer<Lang> Lang { get; set; } = default!;

    [Inject]
    private ISnackbar Snackbar { get; set; } = default!;

    private string _password = "";

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
    }

    private void Submit()
    {
        if (_password.IsEmpty())
        {
            Snackbar.Add(Lang["PasswordEmpty"], Severity.Error);
            return;
        }

        var password = JiuLing.CommonLibs.Security.MD5Utils.GetStringValueToLower(_password);
        MudDialog.Close(DialogResult.Ok(password));
        Snackbar.Add(Lang["SaveOk"], Severity.Success);
    }
}