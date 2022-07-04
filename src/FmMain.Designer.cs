
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FmMain));
            this.Tray = new System.Windows.Forms.NotifyIcon(this.components);
            this.TrayMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.主界面ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.锁定ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.退出ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ChkIsHideWindowWhenLaunch = new System.Windows.Forms.CheckBox();
            this.ChkIsAutoMoveMouse = new System.Windows.Forms.CheckBox();
            this.ChkIsHideWindowWhenClose = new System.Windows.Forms.CheckBox();
            this.BtnPassword = new System.Windows.Forms.Button();
            this.LblPasswordTip = new System.Windows.Forms.Label();
            this.TrayMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // Tray
            // 
            this.Tray.ContextMenuStrip = this.TrayMenu;
            this.Tray.Icon = ((System.Drawing.Icon)(resources.GetObject("Tray.Icon")));
            this.Tray.Text = AppBase.Name;
            this.Tray.Visible = true;
            this.Tray.Click += new System.EventHandler(this.Tray_Click);
            this.Tray.DoubleClick += new System.EventHandler(this.Tray_DoubleClick);
            // 
            // TrayMenu
            // 
            this.TrayMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.主界面ToolStripMenuItem,
            this.锁定ToolStripMenuItem,
            this.退出ToolStripMenuItem});
            this.TrayMenu.Name = "contextMenuStrip1";
            this.TrayMenu.Size = new System.Drawing.Size(137, 70);
            // 
            // 主界面ToolStripMenuItem
            // 
            this.主界面ToolStripMenuItem.Name = "主界面ToolStripMenuItem";
            this.主界面ToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.主界面ToolStripMenuItem.Text = "显示主窗口";
            this.主界面ToolStripMenuItem.Click += new System.EventHandler(this.主界面ToolStripMenuItem_Click);
            // 
            // 锁定ToolStripMenuItem
            // 
            this.锁定ToolStripMenuItem.Name = "锁定ToolStripMenuItem";
            this.锁定ToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.锁定ToolStripMenuItem.Text = "屏幕锁定";
            this.锁定ToolStripMenuItem.Click += new System.EventHandler(this.锁定ToolStripMenuItem_Click);
            // 
            // 退出ToolStripMenuItem
            // 
            this.退出ToolStripMenuItem.Name = "退出ToolStripMenuItem";
            this.退出ToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.退出ToolStripMenuItem.Text = "退出";
            this.退出ToolStripMenuItem.Click += new System.EventHandler(this.退出ToolStripMenuItem_Click);
            // 
            // ChkIsHideWindowWhenLaunch
            // 
            this.ChkIsHideWindowWhenLaunch.AutoSize = true;
            this.ChkIsHideWindowWhenLaunch.Location = new System.Drawing.Point(12, 12);
            this.ChkIsHideWindowWhenLaunch.Name = "ChkIsHideWindowWhenLaunch";
            this.ChkIsHideWindowWhenLaunch.Size = new System.Drawing.Size(135, 21);
            this.ChkIsHideWindowWhenLaunch.TabIndex = 0;
            this.ChkIsHideWindowWhenLaunch.Text = "启动后最小化到托盘";
            this.ChkIsHideWindowWhenLaunch.UseVisualStyleBackColor = true;
            this.ChkIsHideWindowWhenLaunch.CheckedChanged += new System.EventHandler(this.ChkIsHideWindowWhenLaunch_CheckedChanged);
            // 
            // ChkIsAutoMoveMouse
            // 
            this.ChkIsAutoMoveMouse.AutoSize = true;
            this.ChkIsAutoMoveMouse.Location = new System.Drawing.Point(12, 66);
            this.ChkIsAutoMoveMouse.Name = "ChkIsAutoMoveMouse";
            this.ChkIsAutoMoveMouse.Size = new System.Drawing.Size(128, 21);
            this.ChkIsAutoMoveMouse.TabIndex = 2;
            this.ChkIsAutoMoveMouse.Text = "禁用Windows锁屏";
            this.ChkIsAutoMoveMouse.UseVisualStyleBackColor = true;
            this.ChkIsAutoMoveMouse.CheckedChanged += new System.EventHandler(this.ChkIsAutoMoveMouse_CheckedChanged);
            // 
            // ChkIsHideWindowWhenClose
            // 
            this.ChkIsHideWindowWhenClose.AutoSize = true;
            this.ChkIsHideWindowWhenClose.Location = new System.Drawing.Point(12, 39);
            this.ChkIsHideWindowWhenClose.Name = "ChkIsHideWindowWhenClose";
            this.ChkIsHideWindowWhenClose.Size = new System.Drawing.Size(159, 21);
            this.ChkIsHideWindowWhenClose.TabIndex = 1;
            this.ChkIsHideWindowWhenClose.Text = "关闭窗口时最小化到托盘";
            this.ChkIsHideWindowWhenClose.UseVisualStyleBackColor = true;
            this.ChkIsHideWindowWhenClose.CheckedChanged += new System.EventHandler(this.ChkIsHideWindowWhenClose_CheckedChanged);
            // 
            // BtnPassword
            // 
            this.BtnPassword.Location = new System.Drawing.Point(12, 93);
            this.BtnPassword.Name = "BtnPassword";
            this.BtnPassword.Size = new System.Drawing.Size(75, 23);
            this.BtnPassword.TabIndex = 3;
            this.BtnPassword.Text = "设置密码";
            this.BtnPassword.UseVisualStyleBackColor = true;
            this.BtnPassword.Click += new System.EventHandler(this.BtnPassword_Click);
            // 
            // LblPasswordTip
            // 
            this.LblPasswordTip.AutoSize = true;
            this.LblPasswordTip.Location = new System.Drawing.Point(93, 96);
            this.LblPasswordTip.Name = "LblPasswordTip";
            this.LblPasswordTip.Size = new System.Drawing.Size(75, 17);
            this.LblPasswordTip.TabIndex = 4;
            this.LblPasswordTip.Text = "初始密码为1";
            // 
            // FmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(288, 128);
            this.Controls.Add(this.LblPasswordTip);
            this.Controls.Add(this.BtnPassword);
            this.Controls.Add(this.ChkIsHideWindowWhenClose);
            this.Controls.Add(this.ChkIsAutoMoveMouse);
            this.Controls.Add(this.ChkIsHideWindowWhenLaunch);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "FmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = AppBase.Name;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FmMain_FormClosing);
            this.Load += new System.EventHandler(this.FmMain_Load);
            this.Shown += new System.EventHandler(this.FmMain_Shown);
            this.Resize += new System.EventHandler(this.FmMain_Resize);
            this.TrayMenu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.NotifyIcon Tray;
        private System.Windows.Forms.ContextMenuStrip TrayMenu;
        private System.Windows.Forms.ToolStripMenuItem 主界面ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 退出ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 锁定ToolStripMenuItem;
        private System.Windows.Forms.CheckBox ChkIsHideWindowWhenLaunch;
        private System.Windows.Forms.CheckBox ChkIsAutoMoveMouse;
        private System.Windows.Forms.CheckBox ChkIsHideWindowWhenClose;
        private System.Windows.Forms.Button BtnPassword;
        private System.Windows.Forms.Label LblPasswordTip;
    }
}

