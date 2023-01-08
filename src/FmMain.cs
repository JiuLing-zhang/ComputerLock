using ComputerLock.Hooks;
using ComputerLock.resource;
using JiuLing.AutoUpgrade.Shell;
using JiuLing.CommonLibs.ExtensionMethods;
using JiuLing.CommonLibs.Text;
using JiuLing.Controls.WinForms;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ComputerLock
{
    public partial class FmMain : Form
    {
        public FmMain()
        {
            InitializeComponent();
        }

        private static System.Threading.Mutex _mutex;
        private readonly KeyboardHook _keyboardHook = new KeyboardHook();
        private void FmMain_Load(object sender, EventArgs e)
        {
            this.Text = $"{AppBase.Name} v{AppBase.Version}";
            MessageBoxUtils.SetTitle(AppBase.Name);

            _mutex = new System.Threading.Mutex(true, AppBase.FriendlyName);
            if (!_mutex.WaitOne(0, false))
            {
                MessageBoxUtils.ShowError("程序已经运行");
                Application.Exit();
                return;
            }

            _keyboardHook.KeyPressed += _keyboardHook_KeyPressed;

            LoadAppConfig();
            AppConfigToUi();
            if (AppBase.Config.IsAutoCheckUpdate)
            {
                CheckUpdate(true);
            }
        }

        private void _keyboardHook_KeyPressed(object sender, KeyPressedEventArgs e)
        {
            DoLock();
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
            ChkIsDisableWindowsLock.Checked = AppBase.Config.IsDisableWindowsLock;
            ChkIsAutoCheckUpdate.Checked = AppBase.Config.IsAutoCheckUpdate;
            UpdateAutostartUi();
            UpdatePasswordTip();
            UpdateShortcutKeyForLock();
        }
        private void UpdateAutostartUi()
        {
            ChkIsAutostart.Checked = AutostartHook.IsAutostart();
        }
        private void UpdatePasswordTip()
        {
            if (AppBase.Config.IsPasswordChanged)
            {
                LblPasswordTip.Visible = false;
            }
        }

        public void UpdateShortcutKeyForLock()
        {
            if (AppBase.Config.ShortcutKeyForLock.IsEmpty())
            {
                try
                {
                    _keyboardHook.UnregisterHotKey();
                }
                catch (Exception ex)
                {
                    //MessageBoxUtils.ShowError($"取消快捷键失败：{ex.Message}");
                }

                LblShortcutKeyForLock.Text = "未设置";
            }
            else
            {
                try
                {
                    ModifierKeys keys = 0;
                    if (AppBase.Config.ShortcutKeyForLock.IndexOf("Ctrl") >= 0)
                    {
                        keys = keys | Hooks.ModifierKeys.Control;
                    }
                    if (AppBase.Config.ShortcutKeyForLock.IndexOf("Shift") >= 0)
                    {
                        keys = keys | Hooks.ModifierKeys.Shift;
                    }
                    if (AppBase.Config.ShortcutKeyForLock.IndexOf("Alt") >= 0)
                    {
                        keys = keys | Hooks.ModifierKeys.Alt;
                    }

                    var result = RegexUtils.GetFirst(AppBase.Config.ShortcutKeyForLock, @"\d+");
                    if (result.success == false)
                    {
                        throw new Exception("快捷键配置异常");
                    }

                    Keys key = (Keys)Convert.ToInt32(result.result);
                    _keyboardHook.RegisterHotKey(keys, key);

                    LblShortcutKeyForLock.Text = AppBase.Config.ShortcutKeyDisplayForLock;
                }
                catch (Exception ex)
                {
                    MessageBoxUtils.ShowError($"锁屏快捷键注册失败：{ex.Message}");
                }
            }
        }
        private async Task CheckUpdate(bool isBackgroundCheck)
        {
            await Task.Run(() =>
            {
                var autoUpgradePath = Resource.AutoUpgradePath;
                if (autoUpgradePath.IsEmpty())
                {
                    return;
                }
                AutoUpgradeFactory.Create()
                .UseHttpMode(autoUpgradePath)
                .SetUpgrade(config =>
                {
                    config.IsBackgroundCheck = isBackgroundCheck;
                    config.IsCheckSign = true;
                })
                .Run();
            });
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
                    return;
                }
            }
            _keyboardHook.Dispose();
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
            DoLock();
        }

        private bool _isLocked = false;
        private void DoLock()
        {
            if (_isLocked)
            {
                return;
            }
            _isLocked = true;
            var otherScreens = new List<FmLockScreenBlank>();
            if (Screen.AllScreens.Length > 1)
            {
                for (int i = 1; i <= Screen.AllScreens.Length - 1; i++)
                {
                    var otherScreen = new FmLockScreenBlank();
                    setFormLocation(otherScreen, Screen.AllScreens[i]);
                    otherScreen.Show();
                    otherScreen.Activate();
                    otherScreens.Add(otherScreen);
                }
            }

            var mainScreen = new FmLockScreen();
            setFormLocation(mainScreen, Screen.AllScreens[0]);
            mainScreen.Activate();
            mainScreen.ShowDialog();

            foreach (var screen in otherScreens)
            {
                screen.Close();
            }
            _isLocked = false;
        }

        private void setFormLocation(Form form, Screen screen)
        {
            Rectangle bounds = screen.Bounds;
            form.SetBounds(bounds.X, bounds.Y, bounds.Width, bounds.Height);
        }
        private void ChkIsAutostart_CheckedChanged(object sender, EventArgs e)
        {
            if (ChkIsAutostart.Checked)
            {
                AutostartHook.EnabledAutostart();
            }
            else
            {
                AutostartHook.DisabledAutostart();
            }
        }
        private void ChkIsHideWindowWhenLaunch_CheckedChanged(object sender, EventArgs e)
        {
            AppBase.Config.IsHideWindowWhenLaunch = ChkIsHideWindowWhenLaunch.Checked;
            SaveAppConfig();
        }

        private void ChkIsDisableWindowsLock_CheckedChanged(object sender, EventArgs e)
        {
            AppBase.Config.IsDisableWindowsLock = ChkIsDisableWindowsLock.Checked;
            SaveAppConfig();
        }

        private void ChkIsHideWindowWhenClose_CheckedChanged(object sender, EventArgs e)
        {
            AppBase.Config.IsHideWindowWhenClose = ChkIsHideWindowWhenClose.Checked;
            SaveAppConfig();
        }
        private void ChkIsAutoCheckUpdate_CheckedChanged(object sender, EventArgs e)
        {
            AppBase.Config.IsAutoCheckUpdate = ChkIsAutoCheckUpdate.Checked;
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
            MessageBoxUtils.ShowInfo("修改成功");
        }

        private void LblShortcutKeyForLock_Click(object sender, EventArgs e)
        {
            var fmShortcutKeySetting = new FmShortcutKeySetting();
            if (fmShortcutKeySetting.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            AppBase.Config.ShortcutKeyForLock = fmShortcutKeySetting.ShortcutKey;
            AppBase.Config.ShortcutKeyDisplayForLock = fmShortcutKeySetting.ShortcutKeyDisplay;
            SaveAppConfig();
            UpdateShortcutKeyForLock();
        }

        private void BtnClearShortcutKeyForLock_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确定要删除锁屏快捷键吗", AppBase.Name, MessageBoxButtons.OKCancel) != DialogResult.OK)
            {
                return;
            }
            AppBase.Config.ShortcutKeyForLock = "";
            AppBase.Config.ShortcutKeyDisplayForLock = "";
            SaveAppConfig();
            UpdateShortcutKeyForLock();
        }

        private async void LblCheckUpdate_Click(object sender, EventArgs e)
        {
            await CheckUpdate(false);
        }

        private void FmMain_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.WindowState = FormWindowState.Minimized;
            }
        }

        private void pictureBoxGitHub_Click(object sender, EventArgs e)
        {
            try
            {
                using var p = new Process();
                p.StartInfo.FileName = Resource.GitHubUrl;
                p.StartInfo.UseShellExecute = true;
                p.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"浏览器启动失败：{ex.Message}");
            }
        }

        private void pictureBoxGitHub_MouseHover(object sender, EventArgs e)
        {
            ToolTip toolTip1 = new ToolTip();
            toolTip1.SetToolTip(pictureBoxGitHub, "GitHub : 九零");
        }
    }
}
