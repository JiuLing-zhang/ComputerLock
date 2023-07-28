
namespace ComputerLock
{
    partial class FmLockScreen
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            TxtPassword = new System.Windows.Forms.TextBox();
            label1 = new System.Windows.Forms.Label();
            panel1 = new System.Windows.Forms.Panel();
            LblMessage = new System.Windows.Forms.Label();
            timer1 = new System.Windows.Forms.Timer(components);
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // TxtPassword
            // 
            TxtPassword.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            TxtPassword.Location = new System.Drawing.Point(3, 3);
            TxtPassword.Name = "TxtPassword";
            TxtPassword.PasswordChar = '*';
            TxtPassword.Size = new System.Drawing.Size(162, 23);
            TxtPassword.TabIndex = 0;
            TxtPassword.TextChanged += textBox1_TextChanged;
            TxtPassword.KeyDown += TxtPassword_KeyDown;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(3, 29);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(68, 17);
            label1.TabIndex = 1;
            label1.Text = "请输入密码";
            // 
            // panel1
            // 
            panel1.BackColor = System.Drawing.Color.Lavender;
            panel1.Controls.Add(LblMessage);
            panel1.Controls.Add(TxtPassword);
            panel1.Controls.Add(label1);
            panel1.Location = new System.Drawing.Point(72, 92);
            panel1.Name = "panel1";
            panel1.Size = new System.Drawing.Size(168, 52);
            panel1.TabIndex = 2;
            // 
            // LblMessage
            // 
            LblMessage.AutoSize = true;
            LblMessage.Location = new System.Drawing.Point(98, 29);
            LblMessage.Name = "LblMessage";
            LblMessage.Size = new System.Drawing.Size(67, 17);
            LblMessage.TabIndex = 2;
            LblMessage.Text = "3 秒后隐藏";
            // 
            // timer1
            // 
            timer1.Interval = 1000;
            timer1.Tick += timer1_Tick;
            // 
            // FmLockScreen
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(353, 245);
            Controls.Add(panel1);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            Name = "FmLockScreen";
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "FmLockScreen";
            TopMost = true;
            WindowState = System.Windows.Forms.FormWindowState.Maximized;
            FormClosing += FmLockScreen_FormClosing;
            Load += FmLockScreen_Load;
            Resize += FmLockScreen_Resize;
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.TextBox TxtPassword;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label LblMessage;
    }
}