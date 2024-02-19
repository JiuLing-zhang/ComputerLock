using System.Reflection;

namespace ComputerLock.Components;
public partial class AutoUpdate
{
    private bool _isChecking = false;
    private string _currentVersion = "";
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        _currentVersion = Assembly.GetEntryAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion.ToString();
    }

    private async Task CheckUpdateAsync()
    {
        _isChecking = true;
        await InvokeAsync(StateHasChanged);

        var app = JiuLing.AutoUpgrade.Shell.AutoUpgradeFactory.Create();
        await app.UseHttpMode(Resource.AutoUpgradePath)
            .RunAsync();
        _isChecking = false;
        await InvokeAsync(StateHasChanged);
    }
}