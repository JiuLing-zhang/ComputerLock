
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
            主界面ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            锁定ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            退出ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ChkIsHideWindowWhenLaunch = new System.Windows.Forms.CheckBox();
            ChkIsDisableWindowsLock = new System.Windows.Forms.CheckBox();
            ChkIsHideWindowWhenClose = new System.Windows.Forms.CheckBox();
            BtnPassword = new System.Windows.Forms.Button();
            LblPasswordTip = new System.Windows.Forms.Label();
            label1 = new System.Windows.Forms.Label();
            BtnClearShortcutKeyForLock = new System.Windows.Forms.Button();
            LblShortcutKeyForLock = new System.Windows.Forms.Label();
            ChkIsAutoCheckUpdate = new System.Windows.Forms.CheckBox();
            LblCheckUpdate = new System.Windows.Forms.Label();
            ChkIsAutostart = new System.Windows.Forms.CheckBox();
            pictureBoxGitHub = new System.Windows.Forms.PictureBox();
            label2 = new System.Windows.Forms.Label();
            ComboBoxPasswordInputLocation = new System.Windows.Forms.ComboBox();
            LblVersion = new System.Windows.Forms.Label();
            TrayMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBoxGitHub).BeginInit();
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
            TrayMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { 主界面ToolStripMenuItem, 锁定ToolStripMenuItem, 退出ToolStripMenuItem });
            TrayMenu.Name = "contextMenuStrip1";
            TrayMenu.Size = new System.Drawing.Size(137, 70);
            // 
            // 主界面ToolStripMenuItem
            // 
            主界面ToolStripMenuItem.Name = "主界面ToolStripMenuItem";
            主界面ToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            主界面ToolStripMenuItem.Text = "显示主窗口";
            主界面ToolStripMenuItem.Click += 主界面ToolStripMenuItem_Click;
            // 
            // 锁定ToolStripMenuItem
            // 
            锁定ToolStripMenuItem.Name = "锁定ToolStripMenuItem";
            锁定ToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            锁定ToolStripMenuItem.Text = "屏幕锁定";
            锁定ToolStripMenuItem.Click += 锁定ToolStripMenuItem_Click;
            // 
            // 退出ToolStripMenuItem
            // 
            退出ToolStripMenuItem.Name = "退出ToolStripMenuItem";
            退出ToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            退出ToolStripMenuItem.Text = "退出";
            退出ToolStripMenuItem.Click += 退出ToolStripMenuItem_Click;
            // 
            // ChkIsHideWindowWhenLaunch
            // 
            ChkIsHideWindowWhenLaunch.AutoSize = true;
            ChkIsHideWindowWhenLaunch.Location = new System.Drawing.Point(10, 39);
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
            ChkIsDisableWindowsLock.Location = new System.Drawing.Point(10, 93);
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
            ChkIsHideWindowWhenClose.Location = new System.Drawing.Point(10, 66);
            ChkIsHideWindowWhenClose.Name = "ChkIsHideWindowWhenClose";
            ChkIsHideWindowWhenClose.Size = new System.Drawing.Size(159, 21);
            ChkIsHideWindowWhenClose.TabIndex = 2;
            ChkIsHideWindowWhenClose.Text = "关闭窗口时最小化到托盘";
            ChkIsHideWindowWhenClose.UseVisualStyleBackColor = true;
            ChkIsHideWindowWhenClose.CheckedChanged += ChkIsHideWindowWhenClose_CheckedChanged;
            // 
            // BtnPassword
            // 
            BtnPassword.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            BtnPassword.Location = new System.Drawing.Point(286, 204);
            BtnPassword.Name = "BtnPassword";
            BtnPassword.Size = new System.Drawing.Size(75, 23);
            BtnPassword.TabIndex = 10;
            BtnPassword.Text = "设置密码";
            BtnPassword.UseVisualStyleBackColor = true;
            BtnPassword.Click += BtnPassword_Click;
            // 
            // LblPasswordTip
            // 
            LblPasswordTip.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            LblPasswordTip.AutoSize = true;
            LblPasswordTip.Location = new System.Drawing.Point(205, 210);
            LblPasswordTip.Name = "LblPasswordTip";
            LblPasswordTip.Size = new System.Drawing.Size(75, 17);
            LblPasswordTip.TabIndex = 9;
            LblPasswordTip.Text = "初始密码为1";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(10, 178);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(68, 17);
            label1.TabIndex = 6;
            label1.Text = "锁屏快捷键";
            // 
            // BtnClearShortcutKeyForLock
            // 
            BtnClearShortcutKeyForLock.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            BtnClearShortcutKeyForLock.Location = new System.Drawing.Point(286, 175);
            BtnClearShortcutKeyForLock.Name = "BtnClearShortcutKeyForLock";
            BtnClearShortcutKeyForLock.Size = new System.Drawing.Size(75, 23);
            BtnClearShortcutKeyForLock.TabIndex = 8;
            BtnClearShortcutKeyForLock.Text = "清除";
            BtnClearShortcutKeyForLock.UseVisualStyleBackColor = true;
            BtnClearShortcutKeyForLock.Click += BtnClearShortcutKeyForLock_Click;
            // 
            // LblShortcutKeyForLock
            // 
            LblShortcutKeyForLock.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            LblShortcutKeyForLock.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            LblShortcutKeyForLock.Cursor = System.Windows.Forms.Cursors.Hand;
            LblShortcutKeyForLock.ForeColor = System.Drawing.SystemColors.Highlight;
            LblShortcutKeyForLock.Location = new System.Drawing.Point(82, 178);
            LblShortcutKeyForLock.Name = "LblShortcutKeyForLock";
            LblShortcutKeyForLock.Size = new System.Drawing.Size(192, 17);
            LblShortcutKeyForLock.TabIndex = 7;
            LblShortcutKeyForLock.Text = "未设置";
            LblShortcutKeyForLock.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            LblShortcutKeyForLock.Click += LblShortcutKeyForLock_Click;
            // 
            // ChkIsAutoCheckUpdate
            // 
            ChkIsAutoCheckUpdate.AutoSize = true;
            ChkIsAutoCheckUpdate.Location = new System.Drawing.Point(10, 120);
            ChkIsAutoCheckUpdate.Name = "ChkIsAutoCheckUpdate";
            ChkIsAutoCheckUpdate.Size = new System.Drawing.Size(75, 21);
            ChkIsAutoCheckUpdate.TabIndex = 4;
            ChkIsAutoCheckUpdate.Text = "自动更新";
            ChkIsAutoCheckUpdate.UseVisualStyleBackColor = true;
            ChkIsAutoCheckUpdate.CheckedChanged += ChkIsAutoCheckUpdate_CheckedChanged;
            // 
            // LblCheckUpdate
            // 
            LblCheckUpdate.AutoSize = true;
            LblCheckUpdate.Cursor = System.Windows.Forms.Cursors.Hand;
            LblCheckUpdate.ForeColor = System.Drawing.SystemColors.Highlight;
            LblCheckUpdate.Location = new System.Drawing.Point(91, 121);
            LblCheckUpdate.Name = "LblCheckUpdate";
            LblCheckUpdate.Size = new System.Drawing.Size(56, 17);
            LblCheckUpdate.TabIndex = 5;
            LblCheckUpdate.Text = "检查更新";
            LblCheckUpdate.Click += LblCheckUpdate_Click;
            // 
            // ChkIsAutostart
            // 
            ChkIsAutostart.AutoSize = true;
            ChkIsAutostart.Location = new System.Drawing.Point(10, 12);
            ChkIsAutostart.Name = "ChkIsAutostart";
            ChkIsAutostart.Size = new System.Drawing.Size(195, 21);
            ChkIsAutostart.TabIndex = 0;
            ChkIsAutostart.Text = "开机时自动启动（对所有用户）";
            ChkIsAutostart.UseVisualStyleBackColor = true;
            ChkIsAutostart.CheckedChanged += ChkIsAutostart_CheckedChanged;
            // 
            // pictureBoxGitHub
            // 
            pictureBoxGitHub.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            pictureBoxGitHub.Cursor = System.Windows.Forms.Cursors.Hand;
            pictureBoxGitHub.Image = resource.Resource.github;
            pictureBoxGitHub.Location = new System.Drawing.Point(336, 5);
            pictureBoxGitHub.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            pictureBoxGitHub.Name = "pictureBoxGitHub";
            pictureBoxGitHub.Size = new System.Drawing.Size(31, 34);
            pictureBoxGitHub.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            pictureBoxGitHub.TabIndex = 11;
            pictureBoxGitHub.TabStop = false;
            pictureBoxGitHub.Click += pictureBoxGitHub_Click;
            pictureBoxGitHub.MouseHover += pictureBoxGitHub_MouseHover;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(10, 148);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(68, 17);
            label2.TabIndex = 12;
            label2.Text = "密码框位置";
            // 
            // ComboBoxPasswordInputLocation
            // 
            ComboBoxPasswordInputLocation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            ComboBoxPasswordInputLocation.FormattingEnabled = true;
            ComboBoxPasswordInputLocation.Items.AddRange(new object[] { "居中", "左上", "右上", "左下", "右下" });
            ComboBoxPasswordInputLocation.Location = new System.Drawing.Point(84, 144);
            ComboBoxPasswordInputLocation.Name = "ComboBoxPasswordInputLocation";
            ComboBoxPasswordInputLocation.Size = new System.Drawing.Size(63, 25);
            ComboBoxPasswordInputLocation.TabIndex = 13;
            ComboBoxPasswordInputLocation.SelectedIndexChanged += ComboBoxPasswordInputLocation_SelectedIndexChanged;
            // 
            // LblVersion
            // 
            LblVersion.AutoSize = true;
            LblVersion.ForeColor = System.Drawing.SystemColors.Highlight;
            LblVersion.Location = new System.Drawing.Point(10, 210);
            LblVersion.Name = "LblVersion";
            LblVersion.Size = new System.Drawing.Size(44, 17);
            LblVersion.TabIndex = 14;
            LblVersion.Text = "版本号";
            // 
            // FmMain
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(370, 232);
            Controls.Add(LblVersion);
            Controls.Add(ComboBoxPasswordInputLocation);
            Controls.Add(label2);
            Controls.Add(pictureBoxGitHub);
            Controls.Add(LblCheckUpdate);
            Controls.Add(ChkIsAutoCheckUpdate);
            Controls.Add(LblShortcutKeyForLock);
            Controls.Add(BtnClearShortcutKeyForLock);
            Controls.Add(label1);
            Controls.Add(LblPasswordTip);
            Controls.Add(BtnPassword);
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
            ((System.ComponentModel.ISupportInitialize)pictureBoxGitHub).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private System.Windows.Forms.NotifyIcon Tray;
        private System.Windows.Forms.ContextMenuStrip TrayMenu;
        private System.Windows.Forms.ToolStripMenuItem 主界面ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 退出ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 锁定ToolStripMenuItem;
        private System.Windows.Forms.CheckBox ChkIsHideWindowWhenLaunch;
        private System.Windows.Forms.CheckBox ChkIsDisableWindowsLock;
        private System.Windows.Forms.CheckBox ChkIsHideWindowWhenClose;
        private System.Windows.Forms.Button BtnPassword;
        private System.Windows.Forms.Label LblPasswordTip;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button BtnClearShortcutKeyForLock;
        private System.Windows.Forms.Label LblShortcutKeyForLock;
        private System.Windows.Forms.CheckBox ChkIsAutoCheckUpdate;
        private System.Windows.Forms.Label LblCheckUpdate;
        private System.Windows.Forms.CheckBox ChkIsAutostart;
        private System.Windows.Forms.PictureBox pictureBoxGitHub;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox ComboBoxPasswordInputLocation;
        private System.Windows.Forms.Label LblVersion;
    }
}

