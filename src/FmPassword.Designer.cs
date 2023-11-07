
namespace ComputerLock
{
    partial class FmPassword
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FmPassword));
            LblCurrentPassword = new System.Windows.Forms.Label();
            TxtPassword = new System.Windows.Forms.TextBox();
            LblNewPassword = new System.Windows.Forms.Label();
            TxtPasswordNew = new System.Windows.Forms.TextBox();
            LblConfirmPassword = new System.Windows.Forms.Label();
            TxtPasswordNew2 = new System.Windows.Forms.TextBox();
            BtnSave = new System.Windows.Forms.Button();
            SuspendLayout();
            // 
            // LblCurrentPassword
            // 
            LblCurrentPassword.AutoSize = true;
            LblCurrentPassword.Location = new System.Drawing.Point(12, 9);
            LblCurrentPassword.Name = "LblCurrentPassword";
            LblCurrentPassword.Size = new System.Drawing.Size(56, 17);
            LblCurrentPassword.TabIndex = 0;
            LblCurrentPassword.Text = "当前密码";
            // 
            // TxtPassword
            // 
            TxtPassword.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            TxtPassword.Location = new System.Drawing.Point(12, 29);
            TxtPassword.Name = "TxtPassword";
            TxtPassword.PasswordChar = '*';
            TxtPassword.Size = new System.Drawing.Size(178, 23);
            TxtPassword.TabIndex = 0;
            // 
            // LblNewPassword
            // 
            LblNewPassword.AutoSize = true;
            LblNewPassword.Location = new System.Drawing.Point(12, 55);
            LblNewPassword.Name = "LblNewPassword";
            LblNewPassword.Size = new System.Drawing.Size(44, 17);
            LblNewPassword.TabIndex = 0;
            LblNewPassword.Text = "新密码";
            // 
            // TxtPasswordNew
            // 
            TxtPasswordNew.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            TxtPasswordNew.Location = new System.Drawing.Point(12, 75);
            TxtPasswordNew.Name = "TxtPasswordNew";
            TxtPasswordNew.PasswordChar = '*';
            TxtPasswordNew.Size = new System.Drawing.Size(178, 23);
            TxtPasswordNew.TabIndex = 1;
            // 
            // LblConfirmPassword
            // 
            LblConfirmPassword.AutoSize = true;
            LblConfirmPassword.Location = new System.Drawing.Point(12, 101);
            LblConfirmPassword.Name = "LblConfirmPassword";
            LblConfirmPassword.Size = new System.Drawing.Size(56, 17);
            LblConfirmPassword.TabIndex = 0;
            LblConfirmPassword.Text = "确认密码";
            // 
            // TxtPasswordNew2
            // 
            TxtPasswordNew2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            TxtPasswordNew2.Location = new System.Drawing.Point(12, 121);
            TxtPasswordNew2.Name = "TxtPasswordNew2";
            TxtPasswordNew2.PasswordChar = '*';
            TxtPasswordNew2.Size = new System.Drawing.Size(178, 23);
            TxtPasswordNew2.TabIndex = 2;
            // 
            // BtnSave
            // 
            BtnSave.Location = new System.Drawing.Point(60, 153);
            BtnSave.Name = "BtnSave";
            BtnSave.Size = new System.Drawing.Size(75, 23);
            BtnSave.TabIndex = 3;
            BtnSave.Text = "修改";
            BtnSave.UseVisualStyleBackColor = true;
            BtnSave.Click += BtnSave_Click;
            // 
            // FmPassword
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(202, 184);
            Controls.Add(BtnSave);
            Controls.Add(TxtPasswordNew2);
            Controls.Add(LblConfirmPassword);
            Controls.Add(TxtPasswordNew);
            Controls.Add(LblNewPassword);
            Controls.Add(TxtPassword);
            Controls.Add(LblCurrentPassword);
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FmPassword";
            ShowInTaskbar = false;
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "设置密码";
            Load += FmPassword_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label LblCurrentPassword;
        private System.Windows.Forms.TextBox TxtPassword;
        private System.Windows.Forms.Label LblNewPassword;
        private System.Windows.Forms.TextBox TxtPasswordNew;
        private System.Windows.Forms.Label LblConfirmPassword;
        private System.Windows.Forms.TextBox TxtPasswordNew2;
        private System.Windows.Forms.Button BtnSave;
    }
}