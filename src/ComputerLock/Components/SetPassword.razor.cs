namespace ComputerLock.Components;
public partial class SetPassword
{
    [Inject]
    private IStringLocalizer<Lang> Lang { get; set; } = default!;

    [Inject]
    private ISnackbar Snackbar { get; set; } = default!;


    private bool _isShow;
    private string _password = "";
    private InputType _passwordInputType = InputType.Password;
    private string _passwordInputIcon = Icons.Material.Filled.VisibilityOff;

    [Parameter]
    public EventCallback<string> PasswordSetFinished { get; set; }

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
    }

    private void ShowPassword()
    {
        if (_isShow)
        {
            _isShow = false;
            _passwordInputIcon = Icons.Material.Filled.VisibilityOff;
            _passwordInputType = InputType.Password;
        }
        else
        {
            _isShow = true;
            _passwordInputIcon = Icons.Material.Filled.Visibility;
            _passwordInputType = InputType.Text;
        }
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