using Microsoft.AspNetCore.Components.Web;
using System.Text;
using DialogResult = MudBlazor.DialogResult;

namespace ComputerLock.Components;
public partial class ShortcutKeySetting
{
    [CascadingParameter]
    private IMudDialogInstance MudDialog { get; set; } = default!;

    private string _text = default!;
    private string _key = "";
    private string _displayText = "";

    [Inject]
    private IStringLocalizer<Lang> Lang { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        _text = Lang["EnterShortcutKey"];
    }

    private Task OnKeyDown(KeyboardEventArgs value)
    {
        _key = "";

        byte[] buffer = Encoding.ASCII.GetBytes(value.Key);
        if (buffer.Length != 1)
        {
            _text = Lang["EnterShortcutKey"];
            _key = "";
            return Task.CompletedTask;
        }

        var ascii = buffer[0];

        if (value.CtrlKey)
        {
            _key = "Ctrl + ";
        }
        if (value.ShiftKey)
        {
            _key += "Shift + ";
        }
        if (value.AltKey)
        {
            _key += "Alt + ";
        }

        if (ascii >= 97 && ascii <= 122)
        {
            ascii = (byte)char.ToUpper((char)ascii);
        }

        if ((ascii >= 48 && ascii <= 57) || (ascii >= 65 && ascii <= 90))
        {
            _displayText = _key + $"{value.Key}";
            _text = _displayText;
            _key += $"{ascii}";
        }
        else
        {
            _text = Lang["EnterShortcutKey"];
            _key = "";
        }
        return Task.CompletedTask;
    }

    private void Submit()
    {
        MudDialog.Close(DialogResult.Ok(new ShortcutKey(_key, _displayText)));
    }
    private void Cancel() => MudDialog.Cancel();
}