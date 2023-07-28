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
    public partial class FmLockScreenBlank : Form
    {
        public event EventHandler<EventArgs>? OnDeviceInput;
        public FmLockScreenBlank()
        {
            InitializeComponent();
        }

        private void FmLockScreenBlank_Load(object sender, EventArgs e)
        {
        }

        protected override void OnPaintBackground(PaintEventArgs e) { /* Ignore */ }

        private void FmLockScreenBlank_KeyDown(object sender, KeyEventArgs e)
        {
            OnDeviceInput?.Invoke(this, EventArgs.Empty);
        }

        private void FmLockScreenBlank_MouseClick(object sender, MouseEventArgs e)
        {
            //这里只响应左键
            //因为为了实现禁用 Windows 锁屏，会定时点击鼠标右键
            if (e.Button == MouseButtons.Left)
            {
                OnDeviceInput?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
