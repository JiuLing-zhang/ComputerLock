using ComputerLock.Hooks;
using ComputerLock.resource;
using JiuLing.AutoUpgrade.Shared;
using JiuLing.AutoUpgrade.Shell;
using JiuLing.CommonLibs.ExtensionMethods;
using JiuLing.CommonLibs.Text;
using JiuLing.Controls.WinForms;
using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Resources;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ComputerLock;
public partial class FmMain : Form
{
    private ResourceManager _resources = new ResourceManager("ComputerLock.resource.lang.FmMain", typeof(FmMain).Assembly);
    private static System.Threading.Mutex _mutex;
    private readonly KeyboardHook _keyboardHook = new KeyboardHook();
    public FmMain()
    {
        InitializeComponent();

        LoadAppConfig();
        AppConfigToUi();
    }
    private async void FmMain_Load(object sender, EventArgs e)
    {
        _mutex = new System.Threading.Mutex(true, AppBase.FriendlyName);
        if (!_mutex.WaitOne(0, false))
        {
            MessageBoxUtils.ShowError(_resources.GetString("ProgramRun", CultureInfo.CurrentCulture));
            Application.Exit();
            return;
        }
        var version = AppBase.Version;
        LblVersion.Text = $"v{version.Substring(0, version.LastIndexOf("."))}";

        _keyboardHook.KeyPressed += _keyboardHook_KeyPressed;

        if (AppBase.Config.IsAutoCheckUpdate)
        {
            await CheckUpdateAsync(true);
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
        var directory = Path.GetDirectoryName(AppBase.ConfigPath) ?? throw new ArgumentException(_resources.GetString("ConfigFilePathError", CultureInfo.CurrentCulture));
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
        ChkIsHidePasswordWindow.Checked = AppBase.Config.IsHidePasswordWindow;
        UpdateAutostartUi();
        UpdateLangLocation();
        UpdatePasswordInputLocation();
        UpdatePasswordTip();
        UpdateShortcutKeyForLock();
    }
    private void UpdateAutostartUi()
    {
        ChkIsAutostart.Checked = AutostartHook.IsAutostart();
    }

    private void UpdatePasswordInputLocation()
    {
        ComboBoxPasswordInputLocation.Items.Clear();
        ComboBoxPasswordInputLocation.Items.Add(_resources.GetString("Center", CultureInfo.CurrentCulture));
        ComboBoxPasswordInputLocation.Items.Add(_resources.GetString("LeftTop", CultureInfo.CurrentCulture));
        ComboBoxPasswordInputLocation.Items.Add(_resources.GetString("RightTop", CultureInfo.CurrentCulture));
        ComboBoxPasswordInputLocation.Items.Add(_resources.GetString("LeftBottom", CultureInfo.CurrentCulture));
        ComboBoxPasswordInputLocation.Items.Add(_resources.GetString("RightBottom", CultureInfo.CurrentCulture));

        int selectedIndex = (int)AppBase.Config.PasswordInputLocation;
        ComboBoxPasswordInputLocation.SelectedIndex = selectedIndex;
    }
    private void UpdateLangLocation()
    {
        int selectedIndex = (int)AppBase.Config.Lang;
        ComboBoxLang.SelectedIndex = selectedIndex;
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
            LblShortcutKeyForLock.Text = _resources.GetString("Invalid", CultureInfo.CurrentCulture);
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
                    throw new Exception(_resources.GetString("ShortcutKeyConfigError", CultureInfo.CurrentCulture));
                }

                Keys key = (Keys)Convert.ToInt32(result.result);
                _keyboardHook.RegisterHotKey(keys, key);

                LblShortcutKeyForLock.Text = AppBase.Config.ShortcutKeyDisplayForLock;
            }
            catch (Exception ex)
            {
                MessageBoxUtils.ShowError($"{_resources.GetString("ExRegistFailed", CultureInfo.CurrentCulture)}{ex.Message}");
            }
        }
    }
    private async Task CheckUpdateAsync(bool isBackgroundCheck)
    {
        var autoUpgradePath = Resource.AutoUpgradePath;
        if (autoUpgradePath.IsEmpty())
        {
            return;
        }
        await AutoUpgradeFactory.Create()
           .UseHttpMode(autoUpgradePath)
           .SetUpgrade(config =>
           {
               config.IsBackgroundCheck = isBackgroundCheck;
               config.Theme = ThemeEnum.Light;
               config.IsCheckSign = true;
               config.Lang = AppBase.Config.Lang.ToString();
           })
           .RunAsync();
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
    private void ToolBtnMainWindow_Click(object sender, EventArgs e)
    {
        this.Visible = true;
        this.WindowState = FormWindowState.Normal;
    }
    private void ToolBtnExit_Click(object sender, EventArgs e)
    {
        Application.Exit();
    }

    private void ToolBtnDoLock_Click(object sender, EventArgs e)
    {
        DoLock();
    }

    private void DoLock()
    {
        LockService.GetInstance().Lock();
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
    private void ChkIsHidePasswordWindow_CheckedChanged(object sender, EventArgs e)
    {
        AppBase.Config.IsHidePasswordWindow = ChkIsHidePasswordWindow.Checked;
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
        MessageBoxUtils.ShowInfo(_resources.GetString("SaveOk", CultureInfo.CurrentCulture));
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
        AppBase.Config.ShortcutKeyForLock = "";
        AppBase.Config.ShortcutKeyDisplayForLock = "";
        SaveAppConfig();
        UpdateShortcutKeyForLock();
    }

    private async void LblCheckUpdate_Click(object sender, EventArgs e)
    {
        await CheckUpdateAsync(false);
    }

    private void FmMain_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Escape)
        {
            this.WindowState = FormWindowState.Minimized;
        }
    }
    private void LblGitHub_Click(object sender, EventArgs e)
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

        }
    }
    private void ComboBoxPasswordInputLocation_SelectedIndexChanged(object sender, EventArgs e)
    {
        var selectedIndex = ComboBoxPasswordInputLocation.SelectedIndex;
        if (selectedIndex == -1)
        {
            return;
        }

        if (AppBase.Config.PasswordInputLocation == (ScreenLocationEnum)selectedIndex)
        {
            return;
        }
        AppBase.Config.PasswordInputLocation = (ScreenLocationEnum)selectedIndex;
        SaveAppConfig();
        MessageBoxUtils.ShowInfo(_resources.GetString("Restarting", CultureInfo.CurrentCulture));
    }

    private void ComboBoxLang_SelectedIndexChanged(object sender, EventArgs e)
    {
        var selectedIndex = ComboBoxLang.SelectedIndex;
        if (selectedIndex == -1)
        {
            return;
        }

        AppBase.Config.Lang = (LangEnum)selectedIndex;
        SaveAppConfig();
        ChangeLanguage(AppBase.Config.Lang.ToString());
    }

    private void ChangeLanguage(string lang)
    {
        var cultureInfo = new CultureInfo(lang);
        CultureInfo.CurrentCulture = cultureInfo;
        Thread.CurrentThread.CurrentCulture = cultureInfo;
        Thread.CurrentThread.CurrentUICulture = cultureInfo;
        SetControlLanguage(this);
        UpdatePasswordInputLocation();
        SetSpecificLanguage();
    }
    private void SetSpecificLanguage()
    {
        this.Text = _resources.GetString("Title", CultureInfo.CurrentCulture);
        Tray.Text = _resources.GetString("Title", CultureInfo.CurrentCulture);
        ToolBtnMainWindow.Text = _resources.GetString("ToolBtnMainWindow", CultureInfo.CurrentCulture);
        ToolBtnDoLock.Text = _resources.GetString("ToolBtnDoLock", CultureInfo.CurrentCulture);
        ToolBtnExit.Text = _resources.GetString("ToolBtnExit", CultureInfo.CurrentCulture);
        MessageBoxUtils.SetTitle(_resources.GetString("Title", CultureInfo.CurrentCulture));
    }
    private void SetControlLanguage(Control control)
    {
        foreach (Control childControl in control.Controls)
        {
            if (childControl.HasChildren)
            {
                SetControlLanguage(childControl);
            }
            if (childControl.Name == "Tray")
            {

            }
            string? value = _resources.GetString(childControl.Name, CultureInfo.CurrentCulture);
            if (value == null)
            {
                continue;
            }
            childControl.Text = value;
        }
    }
}