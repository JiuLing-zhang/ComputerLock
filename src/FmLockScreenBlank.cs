using System;
using System.Windows.Forms;

namespace ComputerLock
{
    public partial class FmLockScreenBlank : Form
    {
        private bool _isUnlock = false;
        public event EventHandler<EventArgs>? OnDeviceInput;
        private int _unlockAreaHeigh = 70;
        private int _unlockAreaWidth = 160;
        public FmLockScreenBlank()
        {
            InitializeComponent();
        }

        private void FmLockScreenBlank_Load(object sender, EventArgs e)
        {
        }

        public void Unlock()
        {
            _isUnlock = true;
        }

        protected override void OnPaintBackground(PaintEventArgs e) { /* Ignore */ }

        private void FmLockScreenBlank_KeyDown(object sender, KeyEventArgs e)
        {
            if ((AppBase.Config.PasswordBoxActiveMethod & PasswordBoxActiveMethodEnum.KeyboardDown) != PasswordBoxActiveMethodEnum.KeyboardDown)
            {
                return;
            }
            OnDeviceInput?.Invoke(this, EventArgs.Empty);
        }

        private void FmLockScreenBlank_MouseClick(object sender, MouseEventArgs e)
        {
            //这里只响应左键
            //因为为了实现禁用 Windows 锁屏，会定时点击鼠标右键
            if (e.Button != MouseButtons.Left)
            {
                return;
            }
            if ((AppBase.Config.PasswordBoxActiveMethod & PasswordBoxActiveMethodEnum.MouseDown) != PasswordBoxActiveMethodEnum.MouseDown)
            {
                return;
            }

            int xMin;
            int xMax;
            int yMin;
            int yMax;
            switch (AppBase.Config.PasswordInputLocation)
            {
                case ScreenLocationEnum.Center:
                    xMin = this.Width / 2 - _unlockAreaWidth / 2;
                    xMax = this.Width / 2 + _unlockAreaWidth / 2;
                    yMin = this.Height / 2 - _unlockAreaHeigh / 2;
                    yMax = this.Height / 2 + _unlockAreaHeigh / 2;
                    break;
                case ScreenLocationEnum.TopLeft:
                    xMin = 0;
                    xMax = _unlockAreaWidth;
                    yMin = 0;
                    yMax = _unlockAreaHeigh;
                    break;
                case ScreenLocationEnum.TopRight:
                    xMin = this.Width - _unlockAreaWidth;
                    xMax = this.Width;
                    yMin = 0;
                    yMax = _unlockAreaHeigh;
                    break;
                case ScreenLocationEnum.BottomLeft:
                    xMin = 0;
                    xMax = _unlockAreaWidth;
                    yMin = this.Height - _unlockAreaHeigh;
                    yMax = this.Height;
                    break;
                case ScreenLocationEnum.BottomRight:
                    xMin = this.Width - _unlockAreaWidth;
                    xMax = this.Width;
                    yMin = this.Height - _unlockAreaHeigh;
                    yMax = this.Height;
                    break;
                default:
                    xMin = this.Width / 2 - _unlockAreaWidth / 2;
                    xMax = this.Width / 2 + _unlockAreaWidth / 2;
                    yMin = this.Height / 2 - _unlockAreaHeigh / 2;
                    yMax = this.Height / 2 + _unlockAreaHeigh / 2;
                    break;
            }

            if (e.X >= xMin && e.X <= xMax && e.Y >= yMin && e.Y <= yMax)
            {
                OnDeviceInput?.Invoke(this, EventArgs.Empty);
            }
        }

        private void FmLockScreenBlank_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!_isUnlock)
            {
                e.Cancel = true;
            }
        }
    }
}
