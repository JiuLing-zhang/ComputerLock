using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerLock
{
    internal class AppBase
    {
        /// <summary>
        /// App名称
        /// </summary>
        public static string Name { get; set; } = "透明锁屏";
 
        private static string FriendlyName { get; set; } = AppDomain.CurrentDomain.FriendlyName;

        /// <summary>
        /// App Data文件夹路径
        /// </summary>
        private static readonly string DataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        /// <summary>
        /// 配置文件路径
        /// </summary>
        public static string ConfigPath { get; set; } = Path.Combine(DataPath, FriendlyName, "config.json");

        public static AppConfigInfo Config { get; set; }
        public static string Version { get; set; } = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version?.ToString();

    }
}
