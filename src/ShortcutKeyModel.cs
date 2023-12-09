namespace ComputerLock;
internal class ShortcutKeyModel
{
    public string ShortcutKey { get; set; }
    public string ShortcutKeyDisplay { get; set; }

    public ShortcutKeyModel(string shortcutKey, string shortcutKeyDisplay)
    {
        ShortcutKey = shortcutKey;
        ShortcutKeyDisplay = shortcutKeyDisplay;
    }
}