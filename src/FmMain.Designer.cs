
namespace ComputerLock
{
    partial class FmMain
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FmMain));
            Tray = new System.Windows.Forms.NotifyIcon(components);
            TrayMenu = new System.Windows.Forms.ContextMenuStrip(components);
            ToolBtnMainWindow = new System.Windows.Forms.ToolStripMenuItem();
            ToolBtnDoLock = new System.Windows.Forms.ToolStripMenuItem();
            ToolBtnExit = new System.Windows.Forms.ToolStripMenuItem();
            ChkIsHideWindowWhenLaunch = new System.Windows.Forms.CheckBox();
            ChkIsDisableWindowsLock = new System.Windows.Forms.CheckBox();
            ChkIsHideWindowWhenClose = new System.Windows.Forms.CheckBox();
            BtnPassword = new System.Windows.Forms.Button();
            LblPasswordTip = new System.Windows.Forms.Label();
            LblLockShortcutKey = new System.Windows.Forms.Label();
            BtnClearShortcutKeyForLock = new System.Windows.Forms.Button();
            LblShortcutKeyForLock = new System.Windows.Forms.Label();
            ChkIsAutoCheckUpdate = new System.Windows.Forms.CheckBox();
            LblCheckUpdate = new System.Windows.Forms.Label();
            ChkIsAutostart = new System.Windows.Forms.CheckBox();
            LblPwdLocation = new System.Windows.Forms.Label();
            ComboBoxPasswordInputLocation = new System.Windows.Forms.ComboBox();
            LblVersion = new System.Windows.Forms.Label();
            ChkIsHidePasswordWindow = new System.Windows.Forms.CheckBox();
            ComboBoxLang = new System.Windows.Forms.ComboBox();
            label3 = new System.Windows.Forms.Label();
            flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            flowLayoutPanel3 = new System.Windows.Forms.FlowLayoutPanel();
            flowLayoutPanel4 = new System.Windows.Forms.FlowLayoutPanel();
            LblGitHub = new System.Windows.Forms.Label();
            flowLayoutPanel5 = new System.Windows.Forms.FlowLayoutPanel();
            ChkMouseDownActivePwd = new System.Windows.Forms.CheckBox();
            ChkKeyboardDownActivePwd = new System.Windows.Forms.CheckBox();
            TrayMenu.SuspendLayout();
            flowLayoutPanel1.SuspendLayout();
            flowLayoutPanel2.SuspendLayout();
            flowLayoutPanel3.SuspendLayout();
            flowLayoutPanel4.SuspendLayout();
            flowLayoutPanel5.SuspendLayout();
            SuspendLayout();
            // 
            // Tray
            // 
            Tray.ContextMenuStrip = TrayMenu;
            Tray.Icon = (System.Drawing.Icon)resources.GetObject("Tray.Icon");
            Tray.Text = "透明锁屏";
            Tray.Visible = true;
            Tray.Click += Tray_Click;
            Tray.DoubleClick += Tray_DoubleClick;
            // 
            // TrayMenu
            // 
            TrayMenu.ImageScalingSize = new System.Drawing.Size(20, 20);
            TrayMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { ToolBtnMainWindow, ToolBtnDoLock, ToolBtnExit });
            TrayMenu.Name = "contextMenuStrip1";
            TrayMenu.Size = new System.Drawing.Size(137, 70);
            // 
            // ToolBtnMainWindow
            // 
            ToolBtnMainWindow.Name = "ToolBtnMainWindow";
            ToolBtnMainWindow.Size = new System.Drawing.Size(136, 22);
            ToolBtnMainWindow.Text = "显示主窗口";
            ToolBtnMainWindow.Click += ToolBtnMainWindow_Click;
            // 
            // ToolBtnDoLock
            // 
            ToolBtnDoLock.Name = "ToolBtnDoLock";
            ToolBtnDoLock.Size = new System.Drawing.Size(136, 22);
            ToolBtnDoLock.Text = "屏幕锁定";
            ToolBtnDoLock.Click += ToolBtnDoLock_Click;
            // 
            // ToolBtnExit
            // 
            ToolBtnExit.Name = "ToolBtnExit";
            ToolBtnExit.Size = new System.Drawing.Size(136, 22);
            ToolBtnExit.Text = "退出";
            ToolBtnExit.Click += ToolBtnExit_Click;
            // 
            // ChkIsHideWindowWhenLaunch
            // 
            ChkIsHideWindowWhenLaunch.AutoSize = true;
            ChkIsHideWindowWhenLaunch.Location = new System.Drawing.Point(10, 35);
            ChkIsHideWindowWhenLaunch.Name = "ChkIsHideWindowWhenLaunch";
            ChkIsHideWindowWhenLaunch.Size = new System.Drawing.Size(135, 21);
            ChkIsHideWindowWhenLaunch.TabIndex = 1;
            ChkIsHideWindowWhenLaunch.Text = "启动后最小化到托盘";
            ChkIsHideWindowWhenLaunch.UseVisualStyleBackColor = true;
            ChkIsHideWindowWhenLaunch.CheckedChanged += ChkIsHideWindowWhenLaunch_CheckedChanged;
            // 
            // ChkIsDisableWindowsLock
            // 
            ChkIsDisableWindowsLock.AutoSize = true;
            ChkIsDisableWindowsLock.Location = new System.Drawing.Point(10, 117);
            ChkIsDisableWindowsLock.Name = "ChkIsDisableWindowsLock";
            ChkIsDisableWindowsLock.Size = new System.Drawing.Size(128, 21);
            ChkIsDisableWindowsLock.TabIndex = 3;
            ChkIsDisableWindowsLock.Text = "禁用Windows锁屏";
            ChkIsDisableWindowsLock.UseVisualStyleBackColor = true;
            ChkIsDisableWindowsLock.CheckedChanged += ChkIsDisableWindowsLock_CheckedChanged;
            // 
            // ChkIsHideWindowWhenClose
            // 
            ChkIsHideWindowWhenClose.AutoSize = true;
            ChkIsHideWindowWhenClose.Location = new System.Drawing.Point(10, 62);
            ChkIsHideWindowWhenClose.Name = "ChkIsHideWindowWhenClose";
            ChkIsHideWindowWhenClose.Size = new System.Drawing.Size(159, 21);
            ChkIsHideWindowWhenClose.TabIndex = 2;
            ChkIsHideWindowWhenClose.Text = "关闭窗口时最小化到托盘";
            ChkIsHideWindowWhenClose.UseVisualStyleBackColor = true;
            ChkIsHideWindowWhenClose.CheckedChanged += ChkIsHideWindowWhenClose_CheckedChanged;
            // 
            // BtnPassword
            // 
            BtnPassword.Anchor = System.Windows.Forms.AnchorStyles.None;
            BtnPassword.AutoSize = true;
            BtnPassword.Location = new System.Drawing.Point(184, 3);
            BtnPassword.Name = "BtnPassword";
            BtnPassword.Size = new System.Drawing.Size(66, 27);
            BtnPassword.TabIndex = 10;
            BtnPassword.Text = "设置密码";
            BtnPassword.UseVisualStyleBackColor = true;
            BtnPassword.Click += BtnPassword_Click;
            // 
            // LblPasswordTip
            // 
            LblPasswordTip.Anchor = System.Windows.Forms.AnchorStyles.None;
            LblPasswordTip.AutoSize = true;
            LblPasswordTip.Location = new System.Drawing.Point(103, 8);
            LblPasswordTip.Name = "LblPasswordTip";
            LblPasswordTip.Size = new System.Drawing.Size(75, 17);
            LblPasswordTip.TabIndex = 9;
            LblPasswordTip.Text = "初始密码为1";
            // 
            // LblLockShortcutKey
            // 
            LblLockShortcutKey.Anchor = System.Windows.Forms.AnchorStyles.None;
            LblLockShortcutKey.AutoSize = true;
            LblLockShortcutKey.Location = new System.Drawing.Point(3, 8);
            LblLockShortcutKey.Name = "LblLockShortcutKey";
            LblLockShortcutKey.Size = new System.Drawing.Size(68, 17);
            LblLockShortcutKey.TabIndex = 6;
            LblLockShortcutKey.Text = "锁屏快捷键";
            // 
            // BtnClearShortcutKeyForLock
            // 
            BtnClearShortcutKeyForLock.Anchor = System.Windows.Forms.AnchorStyles.None;
            BtnClearShortcutKeyForLock.AutoSize = true;
            BtnClearShortcutKeyForLock.Location = new System.Drawing.Point(258, 3);
            BtnClearShortcutKeyForLock.Name = "BtnClearShortcutKeyForLock";
            BtnClearShortcutKeyForLock.Size = new System.Drawing.Size(75, 27);
            BtnClearShortcutKeyForLock.TabIndex = 8;
            BtnClearShortcutKeyForLock.Text = "清除";
            BtnClearShortcutKeyForLock.UseVisualStyleBackColor = true;
            BtnClearShortcutKeyForLock.Click += BtnClearShortcutKeyForLock_Click;
            // 
            // LblShortcutKeyForLock
            // 
            LblShortcutKeyForLock.Anchor = System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            LblShortcutKeyForLock.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            LblShortcutKeyForLock.Cursor = System.Windows.Forms.Cursors.Hand;
            LblShortcutKeyForLock.ForeColor = System.Drawing.SystemColors.Highlight;
            LblShortcutKeyForLock.Location = new System.Drawing.Point(77, 5);
            LblShortcutKeyForLock.Name = "LblShortcutKeyForLock";
            LblShortcutKeyForLock.Size = new System.Drawing.Size(175, 23);
            LblShortcutKeyForLock.TabIndex = 7;
            LblShortcutKeyForLock.Text = "未设置";
            LblShortcutKeyForLock.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            LblShortcutKeyForLock.Click += LblShortcutKeyForLock_Click;
            // 
            // ChkIsAutoCheckUpdate
            // 
            ChkIsAutoCheckUpdate.Anchor = System.Windows.Forms.AnchorStyles.None;
            ChkIsAutoCheckUpdate.AutoSize = true;
            ChkIsAutoCheckUpdate.Location = new System.Drawing.Point(3, 3);
            ChkIsAutoCheckUpdate.Name = "ChkIsAutoCheckUpdate";
            ChkIsAutoCheckUpdate.Size = new System.Drawing.Size(75, 21);
            ChkIsAutoCheckUpdate.TabIndex = 4;
            ChkIsAutoCheckUpdate.Text = "自动更新";
            ChkIsAutoCheckUpdate.UseVisualStyleBackColor = true;
            ChkIsAutoCheckUpdate.CheckedChanged += ChkIsAutoCheckUpdate_CheckedChanged;
            // 
            // LblCheckUpdate
            // 
            LblCheckUpdate.Anchor = System.Windows.Forms.AnchorStyles.None;
            LblCheckUpdate.AutoSize = true;
            LblCheckUpdate.Cursor = System.Windows.Forms.Cursors.Hand;
            LblCheckUpdate.ForeColor = System.Drawing.SystemColors.Highlight;
            LblCheckUpdate.Location = new System.Drawing.Point(84, 5);
            LblCheckUpdate.Name = "LblCheckUpdate";
            LblCheckUpdate.Size = new System.Drawing.Size(56, 17);
            LblCheckUpdate.TabIndex = 5;
            LblCheckUpdate.Text = "检查更新";
            LblCheckUpdate.Click += LblCheckUpdate_Click;
            // 
            // ChkIsAutostart
            // 
            ChkIsAutostart.AutoSize = true;
            ChkIsAutostart.Location = new System.Drawing.Point(10, 8);
            ChkIsAutostart.Name = "ChkIsAutostart";
            ChkIsAutostart.Size = new System.Drawing.Size(195, 21);
            ChkIsAutostart.TabIndex = 0;
            ChkIsAutostart.Text = "开机时自动启动（对所有用户）";
            ChkIsAutostart.UseVisualStyleBackColor = true;
            ChkIsAutostart.CheckedChanged += ChkIsAutostart_CheckedChanged;
            // 
            // LblPwdLocation
            // 
            LblPwdLocation.Anchor = System.Windows.Forms.AnchorStyles.None;
            LblPwdLocation.AutoSize = true;
            LblPwdLocation.Location = new System.Drawing.Point(3, 7);
            LblPwdLocation.Name = "LblPwdLocation";
            LblPwdLocation.Size = new System.Drawing.Size(68, 17);
            LblPwdLocation.TabIndex = 12;
            LblPwdLocation.Text = "密码框位置";
            // 
            // ComboBoxPasswordInputLocation
            // 
            ComboBoxPasswordInputLocation.Anchor = System.Windows.Forms.AnchorStyles.None;
            ComboBoxPasswordInputLocation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            ComboBoxPasswordInputLocation.FormattingEnabled = true;
            ComboBoxPasswordInputLocation.Items.AddRange(new object[] { "居中", "左上", "右上", "左下", "右下" });
            ComboBoxPasswordInputLocation.Location = new System.Drawing.Point(77, 3);
            ComboBoxPasswordInputLocation.Name = "ComboBoxPasswordInputLocation";
            ComboBoxPasswordInputLocation.Size = new System.Drawing.Size(102, 25);
            ComboBoxPasswordInputLocation.TabIndex = 13;
            ComboBoxPasswordInputLocation.SelectedIndexChanged += ComboBoxPasswordInputLocation_SelectedIndexChanged;
            // 
            // LblVersion
            // 
            LblVersion.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            LblVersion.AutoSize = true;
            LblVersion.ForeColor = System.Drawing.SystemColors.ControlText;
            LblVersion.Location = new System.Drawing.Point(24, 0);
            LblVersion.Name = "LblVersion";
            LblVersion.Size = new System.Drawing.Size(41, 17);
            LblVersion.TabIndex = 14;
            LblVersion.Text = "v0.0.1";
            LblVersion.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ChkIsHidePasswordWindow
            // 
            ChkIsHidePasswordWindow.AutoSize = true;
            ChkIsHidePasswordWindow.Location = new System.Drawing.Point(10, 142);
            ChkIsHidePasswordWindow.Name = "ChkIsHidePasswordWindow";
            ChkIsHidePasswordWindow.Size = new System.Drawing.Size(111, 21);
            ChkIsHidePasswordWindow.TabIndex = 15;
            ChkIsHidePasswordWindow.Text = "自动隐藏密码框";
            ChkIsHidePasswordWindow.UseVisualStyleBackColor = true;
            ChkIsHidePasswordWindow.CheckedChanged += ChkIsHidePasswordWindow_CheckedChanged;
            // 
            // ComboBoxLang
            // 
            ComboBoxLang.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            ComboBoxLang.FormattingEnabled = true;
            ComboBoxLang.Items.AddRange(new object[] { "中文", "en" });
            ComboBoxLang.Location = new System.Drawing.Point(84, 290);
            ComboBoxLang.Name = "ComboBoxLang";
            ComboBoxLang.Size = new System.Drawing.Size(63, 25);
            ComboBoxLang.TabIndex = 17;
            ComboBoxLang.SelectedIndexChanged += ComboBoxLang_SelectedIndexChanged;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(7, 294);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(64, 17);
            label3.TabIndex = 16;
            label3.Text = "语言 Lang";
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            flowLayoutPanel1.Controls.Add(ChkIsAutoCheckUpdate);
            flowLayoutPanel1.Controls.Add(LblCheckUpdate);
            flowLayoutPanel1.Location = new System.Drawing.Point(7, 87);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new System.Drawing.Size(399, 31);
            flowLayoutPanel1.TabIndex = 18;
            // 
            // flowLayoutPanel2
            // 
            flowLayoutPanel2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            flowLayoutPanel2.Controls.Add(LblPwdLocation);
            flowLayoutPanel2.Controls.Add(ComboBoxPasswordInputLocation);
            flowLayoutPanel2.Location = new System.Drawing.Point(7, 225);
            flowLayoutPanel2.Name = "flowLayoutPanel2";
            flowLayoutPanel2.Size = new System.Drawing.Size(399, 31);
            flowLayoutPanel2.TabIndex = 19;
            // 
            // flowLayoutPanel3
            // 
            flowLayoutPanel3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            flowLayoutPanel3.Controls.Add(LblLockShortcutKey);
            flowLayoutPanel3.Controls.Add(LblShortcutKeyForLock);
            flowLayoutPanel3.Controls.Add(BtnClearShortcutKeyForLock);
            flowLayoutPanel3.Location = new System.Drawing.Point(7, 256);
            flowLayoutPanel3.Name = "flowLayoutPanel3";
            flowLayoutPanel3.Size = new System.Drawing.Size(399, 31);
            flowLayoutPanel3.TabIndex = 20;
            // 
            // flowLayoutPanel4
            // 
            flowLayoutPanel4.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            flowLayoutPanel4.Controls.Add(BtnPassword);
            flowLayoutPanel4.Controls.Add(LblPasswordTip);
            flowLayoutPanel4.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            flowLayoutPanel4.Location = new System.Drawing.Point(153, 287);
            flowLayoutPanel4.Name = "flowLayoutPanel4";
            flowLayoutPanel4.Size = new System.Drawing.Size(253, 31);
            flowLayoutPanel4.TabIndex = 21;
            // 
            // LblGitHub
            // 
            LblGitHub.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            LblGitHub.AutoSize = true;
            LblGitHub.Cursor = System.Windows.Forms.Cursors.Hand;
            LblGitHub.ForeColor = System.Drawing.SystemColors.Highlight;
            LblGitHub.Location = new System.Drawing.Point(17, 17);
            LblGitHub.Name = "LblGitHub";
            LblGitHub.Size = new System.Drawing.Size(48, 17);
            LblGitHub.TabIndex = 22;
            LblGitHub.Text = "GitHub";
            LblGitHub.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            LblGitHub.Click += LblGitHub_Click;
            // 
            // flowLayoutPanel5
            // 
            flowLayoutPanel5.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            flowLayoutPanel5.Controls.Add(LblVersion);
            flowLayoutPanel5.Controls.Add(LblGitHub);
            flowLayoutPanel5.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            flowLayoutPanel5.Location = new System.Drawing.Point(339, 8);
            flowLayoutPanel5.Name = "flowLayoutPanel5";
            flowLayoutPanel5.Size = new System.Drawing.Size(68, 39);
            flowLayoutPanel5.TabIndex = 23;
            // 
            // ChkMouseDownActivePwd
            // 
            ChkMouseDownActivePwd.AutoSize = true;
            ChkMouseDownActivePwd.Location = new System.Drawing.Point(10, 196);
            ChkMouseDownActivePwd.Name = "ChkMouseDownActivePwd";
            ChkMouseDownActivePwd.Size = new System.Drawing.Size(171, 21);
            ChkMouseDownActivePwd.TabIndex = 24;
            ChkMouseDownActivePwd.Text = "鼠标点击密码框位置时显示";
            ChkMouseDownActivePwd.UseVisualStyleBackColor = true;
            ChkMouseDownActivePwd.CheckedChanged += ChkMouseDownActivePwd_CheckedChanged;
            // 
            // ChkKeyboardDownActivePwd
            // 
            ChkKeyboardDownActivePwd.AutoSize = true;
            ChkKeyboardDownActivePwd.Location = new System.Drawing.Point(10, 169);
            ChkKeyboardDownActivePwd.Name = "ChkKeyboardDownActivePwd";
            ChkKeyboardDownActivePwd.Size = new System.Drawing.Size(147, 21);
            ChkKeyboardDownActivePwd.TabIndex = 25;
            ChkKeyboardDownActivePwd.Text = "键盘按下时显示密码框";
            ChkKeyboardDownActivePwd.UseVisualStyleBackColor = true;
            ChkKeyboardDownActivePwd.CheckedChanged += ChkKeyboardDownActivePwd_CheckedChanged;
            // 
            // FmMain
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(409, 322);
            Controls.Add(ChkKeyboardDownActivePwd);
            Controls.Add(ChkMouseDownActivePwd);
            Controls.Add(flowLayoutPanel5);
            Controls.Add(ChkIsHidePasswordWindow);
            Controls.Add(flowLayoutPanel4);
            Controls.Add(flowLayoutPanel3);
            Controls.Add(flowLayoutPanel2);
            Controls.Add(flowLayoutPanel1);
            Controls.Add(ComboBoxLang);
            Controls.Add(label3);
            Controls.Add(ChkIsHideWindowWhenClose);
            Controls.Add(ChkIsDisableWindowsLock);
            Controls.Add(ChkIsAutostart);
            Controls.Add(ChkIsHideWindowWhenLaunch);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            KeyPreview = true;
            MaximizeBox = false;
            Name = "FmMain";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "透明锁屏";
            FormClosing += FmMain_FormClosing;
            Load += FmMain_Load;
            Shown += FmMain_Shown;
            KeyDown += FmMain_KeyDown;
            Resize += FmMain_Resize;
            TrayMenu.ResumeLayout(false);
            flowLayoutPanel1.ResumeLayout(false);
            flowLayoutPanel1.PerformLayout();
            flowLayoutPanel2.ResumeLayout(false);
            flowLayoutPanel2.PerformLayout();
            flowLayoutPanel3.ResumeLayout(false);
            flowLayoutPanel3.PerformLayout();
            flowLayoutPanel4.ResumeLayout(false);
            flowLayoutPanel4.PerformLayout();
            flowLayoutPanel5.ResumeLayout(false);
            flowLayoutPanel5.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private System.Windows.Forms.NotifyIcon Tray;
        private System.Windows.Forms.ContextMenuStrip TrayMenu;
        private System.Windows.Forms.ToolStripMenuItem ToolBtnMainWindow;
        private System.Windows.Forms.ToolStripMenuItem ToolBtnExit;
        private System.Windows.Forms.ToolStripMenuItem ToolBtnDoLock;
        private System.Windows.Forms.CheckBox ChkIsHideWindowWhenLaunch;
        private System.Windows.Forms.CheckBox ChkIsDisableWindowsLock;
        private System.Windows.Forms.CheckBox ChkIsHideWindowWhenClose;
        private System.Windows.Forms.Button BtnPassword;
        private System.Windows.Forms.Label LblPasswordTip;
        private System.Windows.Forms.Label LblLockShortcutKey;
        private System.Windows.Forms.Button BtnClearShortcutKeyForLock;
        private System.Windows.Forms.Label LblShortcutKeyForLock;
        private System.Windows.Forms.CheckBox ChkIsAutoCheckUpdate;
        private System.Windows.Forms.Label LblCheckUpdate;
        private System.Windows.Forms.CheckBox ChkIsAutostart;
        private System.Windows.Forms.Label LblPwdLocation;
        private System.Windows.Forms.ComboBox ComboBoxPasswordInputLocation;
        private System.Windows.Forms.Label LblVersion;
        private System.Windows.Forms.CheckBox ChkIsHidePasswordWindow;
        private System.Windows.Forms.ComboBox ComboBoxLang;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel3;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel4;
        private System.Windows.Forms.Label LblGitHub;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel5;
        private System.Windows.Forms.CheckBox ChkMouseDownActivePwd;
        private System.Windows.Forms.CheckBox ChkKeyboardDownActivePwd;
    }
}

