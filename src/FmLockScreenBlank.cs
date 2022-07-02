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
        public FmLockScreenBlank()
        {
            InitializeComponent();
        }

        private void FmLockScreenBlank_Load(object sender, EventArgs e)
        {

        }

        protected override void OnPaintBackground(PaintEventArgs e) { /* Ignore */ }
    }
}
