using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace ComputerLock
{
    public partial class FmMain : Form
    {
        public FmMain()
        {
            InitializeComponent();
        }

        private void FmMain_Load(object sender, EventArgs e)
        {
            LoadAppConfig();
            AppConfigToUi();
        }
        private void FmMain_Shown(object sender, EventArgs e)
        {
            this.WindowState = AppBase.Config.IsHideWindowWhenLaunch ? FormWindowState.Minimized : FormWindowState.Normal;
        }
        private void LoadAppConfig()
        {
            if (!File.Exists(AppBase.ConfigPath))
            {
                AppBase.Config = new AppConfigInfo();
                SaveAppConfig();
            }
            else
            {
                string json = File.ReadAllText(AppBase.ConfigPath);
                AppBase.Config = System.Text.Json.JsonSerializer.Deserialize<AppConfigInfo>(json);
            }
        }
        private void SaveAppConfig()
        {
            var directory = Path.GetDirectoryName(AppBase.ConfigPath) ?? throw new ArgumentException("配置文件路径异常");
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            string appConfigString = System.Text.Json.JsonSerializer.Serialize(AppBase.Config);
            File.WriteAllText(AppBase.ConfigPath, appConfigString);
        }

        private void AppConfigToUi()
        {
            ChkIsHideWindowWhenLaunch.Checked = AppBase.Config.IsHideWindowWhenLaunch;
            ChkIsHideWindowWhenClose.Checked = AppBase.Config.IsHideWindowWhenClose;
            ChkIsAutoMoveMouse.Checked = AppBase.Config.IsAutoMoveMouse;
            UpdatePasswordTip();
        }
        private void UpdatePasswordTip()
        {
            if (AppBase.Config.IsPasswordChanged)
            {
                LblPasswordTip.Visible = false;
            }
        }

        private void FmMain_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.Visible = false;
            }
        }
        private void FmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                if (AppBase.Config.IsHideWindowWhenClose)
                {
                    this.WindowState = FormWindowState.Minimized;
                    e.Cancel = true;
                }
            }
        }
        private void Tray_Click(object sender, EventArgs e)
        {
            var args = e as MouseEventArgs;
            if (args is not { Button: MouseButtons.Left })
            {
                return;
            }
            this.Visible = true;
            this.WindowState = FormWindowState.Normal;
        }

        private void Tray_DoubleClick(object sender, EventArgs e)
        {
            this.Visible = true;
            this.WindowState = FormWindowState.Normal;
        }
        private void 主界面ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Visible = true;
            this.WindowState = FormWindowState.Normal;
        }
        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void 锁定ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Screen.AllScreens.Length == 1)
            {
                new FmLockScreen().Show();
            }
            else
            {
                var screens = new List<FmLockScreenBlank>();
                for (int i = 1; i <= Screen.AllScreens.Length - 1; i++)
                {
                    var otherScreen = new FmLockScreenBlank();
                    setFormLocation(otherScreen, Screen.AllScreens[i]);
                    otherScreen.Show();

                    screens.Add(otherScreen);
                }

                var mainScreen = new FmLockScreen();
                setFormLocation(mainScreen, Screen.AllScreens[0]);
                mainScreen.ShowDialog();

                foreach (var screen in screens)
                {
                    screen.Close();
                }
            }
        }

        private void setFormLocation(Form form, Screen screen)
        {
            Rectangle bounds = screen.Bounds;
            form.SetBounds(bounds.X, bounds.Y, bounds.Width, bounds.Height);
        }

        private void ChkIsHideWindowWhenLaunch_CheckedChanged(object sender, EventArgs e)
        {
            AppBase.Config.IsHideWindowWhenLaunch = ChkIsHideWindowWhenLaunch.Checked;
            SaveAppConfig();
        }

        private void ChkIsAutoMoveMouse_CheckedChanged(object sender, EventArgs e)
        {
            AppBase.Config.IsAutoMoveMouse = ChkIsAutoMoveMouse.Checked;
            SaveAppConfig();
        }

        private void ChkIsHideWindowWhenClose_CheckedChanged(object sender, EventArgs e)
        {
            AppBase.Config.IsHideWindowWhenClose = ChkIsHideWindowWhenClose.Checked;
            SaveAppConfig();
        }

        private void BtnPassword_Click(object sender, EventArgs e)
        {
            var fmPassword = new FmPassword();
            if (fmPassword.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            AppBase.Config.Password = fmPassword.PasswordNew;
            AppBase.Config.IsPasswordChanged = true;
            SaveAppConfig();
            UpdatePasswordTip();
            MessageBox.Show("修改成功");
            return;
        }
    }
}
