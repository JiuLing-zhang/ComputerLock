using JiuLing.CommonLibs.ExtensionMethods;
using System;
using System.Globalization;
using System.Resources;
using System.Windows.Forms;

namespace ComputerLock;
public partial class FmShortcutKeySetting : Form
{
    private ResourceManager _resources = new ResourceManager("ComputerLock.resource.lang.FmShortcutKeySetting", typeof(FmShortcutKeySetting).Assembly);
    public string ShortcutKey { get; set; }
    public string ShortcutKeyDisplay { get; set; }

    public FmShortcutKeySetting()
    {
        InitializeComponent();

        this.Text = _resources.GetString("Title", CultureInfo.CurrentCulture);
        LblKeys.Text = _resources.GetString("EnterShortcutKey", CultureInfo.CurrentCulture);
        BtnSave.Text = _resources.GetString("Save", CultureInfo.CurrentCulture);
        BtnCancel.Text = _resources.GetString("Cancel", CultureInfo.CurrentCulture);
    }
    private void FmShortcutKeySetting_Load(object sender, EventArgs e)
    {
        this.KeyPreview = true;
    }
    private void BtnSave_Click(object sender, EventArgs e)
    {
        if (ShortcutKey.IsNotEmpty())
        {
            DialogResult = DialogResult.OK;
        }
        this.Close();
    }
    private void BtnCancel_Click(object sender, EventArgs e)
    {
        this.Close();
    }

    private void FmShortcutKeySetting_KeyDown(object sender, KeyEventArgs e)
    {
        ShortcutKey = "";
        if (e.Control)
        {
            ShortcutKey = "Ctrl + ";
        }
        if (e.Shift)
        {
            ShortcutKey = ShortcutKey + "Shift + ";
        }
        if (e.Alt)
        {
            ShortcutKey = ShortcutKey + "Alt + ";
        }
        if ((e.KeyValue >= 65 && e.KeyValue <= 90) || (e.KeyValue >= 48 && e.KeyValue <= 57))
        {
            ShortcutKeyDisplay = ShortcutKey + $"{e.KeyCode}";
            LblKeys.Text = ShortcutKeyDisplay;
            ShortcutKey = ShortcutKey + $"{e.KeyValue}";
        }
        else
        {
            LblKeys.Text = _resources.GetString("EnterShortcutKey", CultureInfo.CurrentCulture);
            ShortcutKey = "";
        }
    }
}