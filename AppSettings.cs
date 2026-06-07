using System.Text.Json;

namespace PdfPageStudio;

public sealed class AppSettings
{
    private const int MaxRecentProjects = 5;

    public List<string> RecentProjects { get; set; } = [];
    public string LastPdfFolder { get; set; } = "";

    public static AppSettings Load()
    {
        try
        {
            if (!File.Exists(SettingsPath))
            {
                return new AppSettings();
            }

            var settings = JsonSerializer.Deserialize<AppSettings>(File.ReadAllText(SettingsPath)) ?? new AppSettings();
            settings.RecentProjects = settings.RecentProjects
                .Where(path => !string.IsNullOrWhiteSpace(path))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .Take(MaxRecentProjects)
                .ToList();
            return settings;
        }
        catch
        {
            return new AppSettings();
        }
    }

    public void AddRecentProject(string path)
    {
        RecentProjects.RemoveAll(item => string.Equals(item, path, StringComparison.OrdinalIgnoreCase));
        RecentProjects.Insert(0, path);

        if (RecentProjects.Count > MaxRecentProjects)
        {
            RecentProjects.RemoveRange(MaxRecentProjects, RecentProjects.Count - MaxRecentProjects);
        }
    }

    public void RemoveRecentProject(string path)
    {
        RecentProjects.RemoveAll(item => string.Equals(item, path, StringComparison.OrdinalIgnoreCase));
    }

    public void Save()
    {
        Directory.CreateDirectory(Path.GetDirectoryName(SettingsPath)!);
        File.WriteAllText(SettingsPath, JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true }));
    }

    private static string SettingsPath => Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
        "PdfPageStudio",
        "settings.json");
}
