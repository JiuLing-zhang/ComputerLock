using Microsoft.AspNetCore.Components.Web;
using System.Text;
using DialogResult = MudBlazor.DialogResult;

namespace ComputerLock.Components;
public partial class ShortcutKeySetting
{
    [CascadingParameter]
    private MudDialogInstance MudDialog { get; set; } = default!;

    private string _shortcutKeyText = default!;
    private string _shortcutKey = "";
    private string _shortcutKeyDisplay = "";

    [Inject]
    private IStringLocalizer<Lang> Lang { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        _shortcutKeyText = Lang["EnterShortcutKey"];
    }

    private Task OnKeyDown(KeyboardEventArgs value)
    {
        _shortcutKey = "";

        byte[] buffer = Encoding.ASCII.GetBytes(value.Key);
        if (buffer.Length != 1)
        {
            _shortcutKeyText = Lang["EnterShortcutKey"];
            _shortcutKey = "";
            return Task.CompletedTask;
        }

        var ascii = buffer[0];

        if (value.CtrlKey)
        {
            _shortcutKey = "Ctrl + ";
        }
        if (value.ShiftKey)
        {
            _shortcutKey += "Shift + ";
        }
        if (value.AltKey)
        {
            _shortcutKey += "Alt + ";
        }

        if (ascii >= 97 && ascii <= 122)
        {
            ascii = (byte)char.ToUpper((char)ascii);
        }

        if ((ascii >= 48 && ascii <= 57) || (ascii >= 65 && ascii <= 90))
        {
            _shortcutKeyDisplay = _shortcutKey + $"{value.Key}";
            _shortcutKeyText = _shortcutKeyDisplay;
            _shortcutKey += $"{ascii}";
        }
        else
        {
            _shortcutKeyText = Lang["EnterShortcutKey"];
            _shortcutKey = "";
        }
        return Task.CompletedTask;
    }

    private void Submit()
    {
        MudDialog.Close(DialogResult.Ok(new ShortcutKeyModel(_shortcutKey, _shortcutKeyDisplay)));
    }
    private void Cancel() => MudDialog.Cancel();
}