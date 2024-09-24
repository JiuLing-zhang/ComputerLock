namespace ComputerLock;
internal class ShortcutKeyModel(string shortcutKey, string shortcutKeyDisplay)
{
    public string ShortcutKey { get; set; } = shortcutKey;
    public string ShortcutKeyDisplay { get; set; } = shortcutKeyDisplay;
}