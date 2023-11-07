using JiuLing.CommonLibs.ExtensionMethods;
using JiuLing.Controls.WinForms;
using System;
using System.Globalization;
using System.Resources;
using System.Windows.Forms;

namespace ComputerLock
{
    public partial class FmPassword : Form
    {
        private ResourceManager _resources = new ResourceManager("ComputerLock.resource.lang.FmPassword", typeof(FmPassword).Assembly);

        /// <summary>
        /// 新密码
        /// </summary>
        public string PasswordNew { get; set; }
        public FmPassword()
        {
            InitializeComponent();

            this.Text = _resources.GetString("Title", CultureInfo.CurrentCulture);
            LblCurrentPassword.Text = _resources.GetString("LblCurrentPassword", CultureInfo.CurrentCulture);
            LblNewPassword.Text = _resources.GetString("LblNewPassword", CultureInfo.CurrentCulture);
            LblConfirmPassword.Text = _resources.GetString("LblConfirmPassword", CultureInfo.CurrentCulture);
            BtnSave.Text = _resources.GetString("BtnSave", CultureInfo.CurrentCulture);
        }

        private void FmPassword_Load(object sender, EventArgs e)
        {

        }
        private void BtnSave_Click(object sender, EventArgs e)
        {
            var password = TxtPassword.Text;
            var passwordNew = TxtPasswordNew.Text;
            if (passwordNew.IsEmpty())
            {
                MessageBoxUtils.ShowError(_resources.GetString("MsgPasswordEmpty", CultureInfo.CurrentCulture));
                return;
            }

            if (passwordNew != TxtPasswordNew2.Text)
            {
                MessageBoxUtils.ShowError(_resources.GetString("MsgPasswordInconsistent", CultureInfo.CurrentCulture));
                return;
            }

            if (AppBase.Config.Password != JiuLing.CommonLibs.Security.MD5Utils.GetStringValueToLower(password))
            {
                MessageBoxUtils.ShowError(_resources.GetString("MsgWrongPassword", CultureInfo.CurrentCulture));
                return;
            }

            PasswordNew = JiuLing.CommonLibs.Security.MD5Utils.GetStringValueToLower(passwordNew);
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
