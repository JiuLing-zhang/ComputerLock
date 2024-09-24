using DialogResult = MudBlazor.DialogResult;

namespace ComputerLock.Components;
public partial class SetPassword
{
    [CascadingParameter]
    private MudDialogInstance MudDialog { get; set; } = default!;

    private string _currentPassword = "";
    private string _newPassword = "";
    private string _confirmPassword = "";

    [Inject]
    private IStringLocalizer<Lang> Lang { get; set; } = default!;

    [Inject]
    private ISnackbar Snackbar { get; set; } = default!;

    [Inject]
    private AppSettings AppSettings { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
    }


    private void Submit()
    {
        if (_newPassword.IsEmpty())
        {
            Snackbar.Add(Lang["PasswordEmpty"], Severity.Error);
            return;
        }

        if (_newPassword != _confirmPassword)
        {
            Snackbar.Add(Lang["PasswordInconsistent"], Severity.Error);
            return;
        }

        if (AppSettings.Password != JiuLing.CommonLibs.Security.MD5Utils.GetStringValueToLower(_currentPassword))
        {

            Snackbar.Add(Lang["WrongPassword"], Severity.Error);

            return;
        }

        var newPassword = JiuLing.CommonLibs.Security.MD5Utils.GetStringValueToLower(_newPassword);
        MudDialog.Close(DialogResult.Ok(newPassword));
        Snackbar.Add(Lang["SaveOk"], Severity.Success);
    }
}