namespace ComputerLock.Models;

public class HotkeyTools(IStringLocalizer<Lang> lang)
{
    public Hotkey? StringKeyToHotkey(string shortcutKey)
    {
        if (shortcutKey.IsEmpty())
        {
            return null;
        }

        HotkeyModifiers modifiers = 0;
        if (shortcutKey.IndexOf("Ctrl") >= 0)
        {
            modifiers |= HotkeyModifiers.Control;
        }
        if (shortcutKey.IndexOf("Shift") >= 0)
        {
            modifiers |= HotkeyModifiers.Shift;
        }
        if (shortcutKey.IndexOf("Alt") >= 0)
        {
            modifiers |= HotkeyModifiers.Alt;
        }

        string[] keyParts = shortcutKey.Split([" + "], StringSplitOptions.None);
        int keyCode = int.Parse(keyParts[^1]);
        Keys key = (Keys)keyCode;
        return new Hotkey(modifiers, key);
    }

    public string StringKeyToDisplay(string shortcutKey)
    {
        if (shortcutKey.IsEmpty())
        {
            return lang["Invalid"];
        }

        string[] keyParts = shortcutKey.Split([" + "], StringSplitOptions.None);
        int keyCode = int.Parse(keyParts[^1]);
        keyParts[^1] = ((Keys)keyCode).ToString();
        return string.Join(" + ", keyParts);
    }
}