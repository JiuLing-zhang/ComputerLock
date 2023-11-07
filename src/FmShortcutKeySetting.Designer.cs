namespace ComputerLock
{
    partial class FmShortcutKeySetting
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
            BtnCancel = new System.Windows.Forms.Button();
            BtnSave = new System.Windows.Forms.Button();
            LblKeys = new System.Windows.Forms.Label();
            SuspendLayout();
            // 
            // BtnCancel
            // 
            BtnCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            BtnCancel.Location = new System.Drawing.Point(196, 96);
            BtnCancel.Name = "BtnCancel";
            BtnCancel.Size = new System.Drawing.Size(75, 23);
            BtnCancel.TabIndex = 2;
            BtnCancel.Text = "取消";
            BtnCancel.UseVisualStyleBackColor = true;
            BtnCancel.Click += BtnCancel_Click;
            // 
            // BtnSave
            // 
            BtnSave.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            BtnSave.Location = new System.Drawing.Point(114, 96);
            BtnSave.Name = "BtnSave";
            BtnSave.Size = new System.Drawing.Size(75, 23);
            BtnSave.TabIndex = 1;
            BtnSave.Text = "确定";
            BtnSave.UseVisualStyleBackColor = true;
            BtnSave.Click += BtnSave_Click;
            // 
            // LblKeys
            // 
            LblKeys.AutoSize = true;
            LblKeys.Font = new System.Drawing.Font("Microsoft YaHei UI", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            LblKeys.Location = new System.Drawing.Point(12, 38);
            LblKeys.Name = "LblKeys";
            LblKeys.Size = new System.Drawing.Size(262, 24);
            LblKeys.TabIndex = 3;
            LblKeys.Text = "请直接在键盘上输入新的快捷键";
            // 
            // FmShortcutKeySetting
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.Color.White;
            ClientSize = new System.Drawing.Size(283, 131);
            Controls.Add(LblKeys);
            Controls.Add(BtnSave);
            Controls.Add(BtnCancel);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            Name = "FmShortcutKeySetting";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "设置快捷键";
            Load += FmShortcutKeySetting_Load;
            KeyDown += FmShortcutKeySetting_KeyDown;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private System.Windows.Forms.Button BtnCancel;
        private System.Windows.Forms.Button BtnSave;
        private System.Windows.Forms.Label LblKeys;
    }
}