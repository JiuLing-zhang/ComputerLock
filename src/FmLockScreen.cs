using JiuLing.CommonLibs.ExtensionMethods;
using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace ComputerLock
{
    public partial class FmLockScreen : Form
    {
        public FmLockScreen()
        {
            InitializeComponent();
        }

        private bool _isLocked;
        private DateTime _hideSelfTime;
        private int _hideSelfSecond = 3;
        public event EventHandler<EventArgs>? OnUnlock;

        /// <summary>
        /// 引用user32.dll动态链接库（windows api），
        /// 使用库中定义 API：SetCursorPos 
        /// </summary>
        [DllImport("user32.dll")]
        private static extern int SetCursorPos(int x, int y);
        /// <summary>
        /// 移动鼠标到指定的坐标点
        /// </summary>
        public void MoveMouseToPoint(Point p)
        {
            SetCursorPos(p.X, p.Y);
        }

        //点击事件
        [DllImport("User32")]
        public static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);
        public const int MOUSEEVENTF_LEFTDOWN = 0x0002;
        public const int MOUSEEVENTF_LEFTUP = 0x0004;
        public const int MOUSEEVENTF_RIGHTDOWN = 0x0008;
        public const int MOUSEEVENTF_RIGHTUP = 0x0010;

        private void FmLockScreen_Load(object sender, EventArgs e)
        {
            timer1.Start();
        }

        public void Open()
        {
            if (this.Visible)
            {
                return;
            }
            _isLocked = true;
            TxtPassword.Text = "";
            LblMessage.Text = $"{_hideSelfSecond} 秒后隐藏";
            LblMessage.Visible = AppBase.Config.IsHidePasswordWindow;
            RefreshHideSelfTime();
            this.Show();
            this.Activate();
        }

        protected override void OnPaintBackground(PaintEventArgs e) { /* Ignore */ }
        private void FmLockScreen_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_isLocked)
            {
                e.Cancel = true;
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            RefreshHideSelfTime();
            var txt = TxtPassword.Text;
            if (txt.IsEmpty())
            {
                return;
            }

            if (AppBase.Config.Password != JiuLing.CommonLibs.Security.MD5Utils.GetStringValueToLower(txt))
            {
                return;
            }

            HideSelf();
            _isLocked = false;
            OnUnlock?.Invoke(this, EventArgs.Empty);
        }
        private void FmLockScreen_Resize(object sender, EventArgs e)
        {
            switch (AppBase.Config.PasswordInputLocation)
            {
                case ScreenLocationEnum.Center:
                    panel1.Top = this.Height / 2 - this.panel1.Height / 2;
                    panel1.Left = this.Width / 2 - this.panel1.Width / 2;
                    break;
                case ScreenLocationEnum.TopLeft:
                    panel1.Top = 0;
                    panel1.Left = 0;
                    break;
                case ScreenLocationEnum.TopRight:
                    panel1.Top = 0;
                    panel1.Left = this.Width - this.panel1.Width;
                    break;
                case ScreenLocationEnum.BottomLeft:
                    panel1.Top = this.Height - this.panel1.Height;
                    panel1.Left = 0;
                    break;
                case ScreenLocationEnum.BottomRight:
                    panel1.Top = this.Height - this.panel1.Height;
                    panel1.Left = this.Width - this.panel1.Width;
                    break;
                default:
                    panel1.Top = this.Height / 2 - this.panel1.Height / 2;
                    panel1.Left = this.Width / 2 - this.panel1.Width / 2;
                    break;
            }
        }

        private void TxtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                TxtPassword.Text = "";
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                if (!_isLocked)
                {
                    return;
                }
                var time = DateTime.Now;
                if (time.Second % 30 == 0)
                {
                    if (AppBase.Config.IsDisableWindowsLock)
                    {
                        DoMoveMouse();
                    }
                }

                if (AppBase.Config.IsHidePasswordWindow)
                {
                    var hideCountdown = Convert.ToInt32(_hideSelfTime.Subtract(time).TotalSeconds);
                    LblMessage.Text = $"{hideCountdown} 秒后隐藏";
                    if (hideCountdown <= 0)
                    {
                        HideSelf();
                    }
                }

            }
            catch (Exception ex)
            {

            }
        }

        private void DoMoveMouse()
        {
            var random = new Random();
            var x = random.Next(0, 100);
            var y = random.Next(0, 100);

            MoveMouseToPoint(new Point(x, y));
            mouse_event(MOUSEEVENTF_RIGHTDOWN | MOUSEEVENTF_RIGHTUP, x, y, 0, 0);
        }
        private void HideSelf()
        {
            if (this.Visible)
            {
                this.Hide();
            }
        }

        private void RefreshHideSelfTime()
        {
            _hideSelfTime = DateTime.Now.AddSeconds(_hideSelfSecond);
        }
    }
}
