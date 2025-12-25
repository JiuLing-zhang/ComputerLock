using ComputerLock.Interfaces;

namespace ComputerLock.Platforms;

internal class WindowsMessageBox(IStringLocalizer<Lang> lang) : IWindowsMessageBox
{
    public void Show(string text)
    {
        MessageBox.Show(text, lang["Title"], MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, System.Windows.Forms.MessageBoxOptions.DefaultDesktopOnly);
    }
}