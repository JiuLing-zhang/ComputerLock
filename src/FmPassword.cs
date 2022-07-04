using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using JiuLing.CommonLibs.ExtensionMethods;
using JiuLing.Controls.WinForms;

namespace ComputerLock
{
    public partial class FmPassword : Form
    {
        public FmPassword()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 新密码
        /// </summary>
        public string PasswordNew { get; set; }
        private void FmPassword_Load(object sender, EventArgs e)
        {

        }
        private void BtnOk_Click(object sender, EventArgs e)
        {
            var password = TxtPassword.Text;
            var passwordNew = TxtPasswordNew.Text;
            if (passwordNew.IsEmpty())
            {
                MessageBoxUtils.ShowError("密码不能为空");
                return;
            }

            if (passwordNew != TxtPasswordNew2.Text)
            {
                MessageBoxUtils.ShowError("新密码与确认密码不一致");
                return;
            }

            if (AppBase.Config.Password != JiuLing.CommonLibs.Security.MD5Utils.GetStringValueToLower(password))
            {
                MessageBoxUtils.ShowError("密码错误");
                return;
            }

            PasswordNew = JiuLing.CommonLibs.Security.MD5Utils.GetStringValueToLower(passwordNew);
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
