using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using JiuLing.CommonLibs.ExtensionMethods;

namespace ComputerLock
{
    public partial class FmLockScreen : Form
    {
        public FmLockScreen()
        {
            InitializeComponent();
        }

        private bool _isLocked = true;

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
        /// <summary>
        /// 设置鼠标的移动范围
        /// </summary>
        public void SetMouseRectangle(Rectangle rectangle)
        {
            System.Windows.Forms.Cursor.Clip = rectangle;
        }
        //点击事件
        [DllImport("User32")]
        //下面这一行对应着下面的点击事件
        //    public extern static void mouse_event(int dwFlags, int dx, int dy, int dwData, IntPtr dwExtraInfo);
        public extern static void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);
        public const int MOUSEEVENTF_LEFTDOWN = 0x2;
        public const int MOUSEEVENTF_LEFTUP = 0x4;
        public enum MouseEventFlags
        {
            Move = 0x0001, //移动鼠标
            LeftDown = 0x0002,//模拟鼠标左键按下
            LeftUp = 0x0004,//模拟鼠标左键抬起
            RightDown = 0x0008,//鼠标右键按下
            RightUp = 0x0010,//鼠标右键抬起
            MiddleDown = 0x0020,//鼠标中键按下 
            MiddleUp = 0x0040,//中键抬起
            Wheel = 0x0800,
            Absolute = 0x8000//标示是否采用绝对坐标
        }
        //鼠标将要到的x，y位置
        public static int loginx, loginy;
        private void FmLockScreen_Load(object sender, EventArgs e)
        {
            TaskManagerHook.DisabledTaskManager();
            timer1.Start();
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
            var txt = TxtPassword.Text;
            if (txt.IsEmpty())
            {
                return;
            }
            if (AppBase.Config.Password == JiuLing.CommonLibs.Security.MD5Utils.GetStringValueToLower(txt))
            {
                _isLocked = false;
                TaskManagerHook.EnabledTaskManager();
                timer1.Stop();
                this.Close();
            }
        }
        private void FmLockScreen_Resize(object sender, EventArgs e)
        {
            this.panel1.Left = this.Width / 2 - this.panel1.Width / 2;
            this.panel1.Top = this.Height / 2 - this.panel1.Height / 2;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                if (!AppBase.Config.IsDisableWindowsLock)
                {
                    return;
                }

                var random = new Random();
                var x = random.Next(0, 100);
                var y = random.Next(0, 100);

                MoveMouseToPoint(new Point(x, y));
                mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, x, y, 0, 0);
            }
            catch (Exception ex)
            {

            }
        }
    }
}
