namespace ComputerLock.Models;

public class HotKey(HotKeyModifiers modifiers, Keys key)
{
    public HotKeyModifiers Modifiers { get; set; } = modifiers;
    public Keys Key { get; set; } = key;
}