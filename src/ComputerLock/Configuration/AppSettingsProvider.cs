using System.IO;
using System.Text.Json;

namespace ComputerLock.Configuration;
public class AppSettingsProvider(IStringLocalizer<Lang> lang)
{
    public AppSettings LoadSettings()
    {
        if (!File.Exists(AppBase.ConfigPath))
        {
            return new AppSettings();
        }
        string json = File.ReadAllText(AppBase.ConfigPath);
        try
        {
            var appSettings = JsonSerializer.Deserialize<AppSettings>(json);
            return appSettings ?? new AppSettings();
        }
        catch (JsonException)
        {
            return new AppSettings();
        }
    }

    public void SaveSettings(AppSettings settings)
    {
        var directory = Path.GetDirectoryName(AppBase.ConfigPath) ?? throw new ArgumentException(lang["ConfigFilePathError"]);
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }
        string appConfigString = System.Text.Json.JsonSerializer.Serialize(settings);
        File.WriteAllText(AppBase.ConfigPath, appConfigString);
    }

    public void RemoveSettings()
    {
        if (File.Exists(AppBase.ConfigPath))
        {
            File.Delete(AppBase.ConfigPath);
        }
    }
}