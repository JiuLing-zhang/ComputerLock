using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace ComputerLock.Configuration;

internal class AppBase
{
    /// <summary>
    /// App路径（包含文件名）
    /// </summary>
    public static string ExecutablePath { get; } = Process.GetCurrentProcess().MainModule.FileName;

    public static string FriendlyName { get; } = AppDomain.CurrentDomain.FriendlyName;

    /// <summary>
    /// App Data文件夹路径
    /// </summary>
    private static readonly string DataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
    /// <summary>
    /// 配置文件路径
    /// </summary>
    public static string ConfigPath { get; } = Path.Combine(DataPath, FriendlyName, "config.json");

    /// <summary>
    /// 版本文件路径
    /// </summary>
    public static string VersionFilePath { get; } = Path.Combine(DataPath, FriendlyName, "current_version");

    /// <summary>
    /// 版本号
    /// </summary>
    public static Version Version { get; } = Assembly.GetExecutingAssembly().GetName().Version ?? throw new InvalidOperationException("App Version");
    public static string VersionString { get; } = Version.ToString();

    /// <summary>
    /// 首次运行标志文件路径
    /// </summary>
    private static string FirstRunFilePath { get; } = Path.Combine(DataPath, FriendlyName, "first_run_flag");

    private static bool? _isFirstRun = null;
    /// <summary>
    /// 是否为首次运行
    /// </summary>
    public static bool IsFirstRun
    {
        get
        {
            if (_isFirstRun == null)
            {
                // 文件不存在 = 首次运行
                _isFirstRun = !File.Exists(FirstRunFilePath);

                // 如果是首次运行，立即创建标记文件，确保本次运行期间再次读取仍为 true
                if (_isFirstRun == true)
                {
                    // 确保目录存在
                    Directory.CreateDirectory(Path.GetDirectoryName(FirstRunFilePath));
                    File.WriteAllText(FirstRunFilePath, DateTime.Now.ToString());
                }
            }
            return _isFirstRun.Value;
        }
    }
}