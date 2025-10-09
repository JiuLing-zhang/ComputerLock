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
}