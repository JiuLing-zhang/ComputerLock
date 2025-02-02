namespace ComputerLock.Models;

public class HotKeyTools(IStringLocalizer<Lang> lang)
{
    public HotKey? StringKeyToHotKey(string shortcutKey)
    {
        if (shortcutKey.IsEmpty())
        {
            return null;
        }

        HotKeyModifiers modifiers = 0;
        if (shortcutKey.IndexOf("Ctrl") >= 0)
        {
            modifiers |= HotKeyModifiers.Control;
        }
        if (shortcutKey.IndexOf("Shift") >= 0)
        {
            modifiers |= HotKeyModifiers.Shift;
        }
        if (shortcutKey.IndexOf("Alt") >= 0)
        {
            modifiers |= HotKeyModifiers.Alt;
        }

        string[] keyParts = shortcutKey.Split([" + "], StringSplitOptions.None);
        int keyCode = int.Parse(keyParts[^1]);
        Keys key = (Keys)keyCode;
        return new HotKey(modifiers, key);
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