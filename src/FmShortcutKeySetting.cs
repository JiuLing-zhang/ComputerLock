using JiuLing.CommonLibs.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ComputerLock
{
    public partial class FmShortcutKeySetting : Form
    {
        public FmShortcutKeySetting()
        {
            InitializeComponent();
        }

        public string ShortcutKey { get; set; }
        public string ShortcutKeyDisplay { get; set; }
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
        private void BtnExit_Click(object sender, EventArgs e)
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
                LblKeys.Text = "请直接在键盘上输入新的快捷键";
                ShortcutKey = "";
            }
        }
    }
}
