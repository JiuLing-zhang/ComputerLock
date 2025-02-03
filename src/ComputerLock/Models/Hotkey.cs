namespace ComputerLock.Models;

public class Hotkey(HotkeyModifiers modifiers, Keys key)
{
    public HotkeyModifiers Modifiers { get; set; } = modifiers;
    public Keys Key { get; set; } = key;
}