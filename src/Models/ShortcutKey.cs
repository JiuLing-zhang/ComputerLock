namespace ComputerLock.Models;
internal class ShortcutKey(string key, string displayText)
{
    public string Key { get; set; } = key;
    public string DisplayText { get; set; } = displayText;
}
