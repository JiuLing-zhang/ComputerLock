using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerLock
{
    internal class AppConfigInfo
    {
        /// <summary>
        /// 关闭窗口时最小化到托盘
        /// </summary>
        public bool IsHideWindowWhenClose { get; set; } = true;

        /// <summary>
        /// 启动后最小化到托盘
        /// </summary>
        public bool IsHideWindowWhenLaunch { get; set; } = true;

        /// <summary>
        /// 自动移动鼠标
        /// </summary>
        public bool IsAutoMoveMouse { get; set; } = true;

        /// <summary>
        /// 默认密码 1
        /// </summary>
        public string Password { get; set; } = "c4ca4238a0b923820dcc509a6f75849b";

        /// <summary>
        /// 初始密码已修改
        /// </summary>
        public bool IsPasswordChanged { get; set; } = false;
    }
}
