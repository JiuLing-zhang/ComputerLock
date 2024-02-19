using System.IO;

namespace ComputerLock;
public class AppSettingWriter(IStringLocalizer<Lang> lang)
{
    public void Save(AppSettings appSettings)
    {
        var directory = Path.GetDirectoryName(AppBase.ConfigPath) ?? throw new ArgumentException(lang["ConfigFilePathError"]);
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }
        string appConfigString = System.Text.Json.JsonSerializer.Serialize(appSettings);
        File.WriteAllText(AppBase.ConfigPath, appConfigString);
    }
}