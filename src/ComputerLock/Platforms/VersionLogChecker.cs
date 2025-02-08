using System.IO;

namespace ComputerLock.Platforms;

internal class VersionLogChecker(Version currentVersion, string versionFilePath)
{
    // 检查是否需要展示更新日志
    public async Task<bool> CheckShowUpdateLogAsync()
    {
        var previousVersion = await GetPreviousVersionAsync();

        if (currentVersion <= previousVersion)
        {
            return false;
        }

        // 目前的逻辑是只自动显示一次更新信息，所以这里直接更新版本信息
        await SaveCurrentVersionAsync(currentVersion);
        return true;
    }

    // 读取上次保存的版本号
    private async Task<Version> GetPreviousVersionAsync()
    {
        if (File.Exists(versionFilePath))
        {
            string versionText = await File.ReadAllTextAsync(versionFilePath);
            if (Version.TryParse(versionText, out var savedVersion))
            {
                return savedVersion;
            }
        }
        // 如果文件不存在或版本无效，则返回一个默认的低版本号
        return new Version("0.0.0.0");
    }

    // 保存当前版本号到本地文件
    private async Task SaveCurrentVersionAsync(Version version)
    {
        string directory = Path.GetDirectoryName(versionFilePath) ?? throw new InvalidOperationException("versionFilePath");
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }
        await File.WriteAllTextAsync(versionFilePath, version.ToString());
    }
}
