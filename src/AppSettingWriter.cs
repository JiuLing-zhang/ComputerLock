using ComputerLock.Resources;
using Microsoft.Extensions.Localization;
using System.IO;

namespace ComputerLock;
public class AppSettingWriter
{
    private readonly IStringLocalizer<Lang> _lang;
    public AppSettingWriter(IStringLocalizer<Lang> lang)
    {
        _lang = lang;
    }
    public void Save(AppSettings appSettings)
    {
        var directory = Path.GetDirectoryName(AppBase.ConfigPath) ?? throw new ArgumentException(_lang["ConfigFilePathError"]);
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }
        string appConfigString = System.Text.Json.JsonSerializer.Serialize(appSettings);
        File.WriteAllText(AppBase.ConfigPath, appConfigString);
    }
}