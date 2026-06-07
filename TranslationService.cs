using System.Globalization;
using System.Reflection;
using System.Resources;

namespace PdfPageStudio;

public static class TranslationService
{
    private static readonly ResourceManager ResourceManager = new(
        "PdfPageStudio.Resources.Strings",
        Assembly.GetExecutingAssembly());

    public static string CurrentLanguage { get; private set; } = "en";

    public static void Initialize(string? language)
    {
        SetLanguage(NormalizeLanguage(language));
    }

    public static void SetLanguage(string language)
    {
        CurrentLanguage = NormalizeLanguage(language);
        var culture = new CultureInfo(CurrentLanguage);
        CultureInfo.CurrentUICulture = culture;
        CultureInfo.CurrentCulture = culture;
    }

    public static string T(string key)
    {
        return ResourceManager.GetString(key, CultureInfo.CurrentUICulture) ?? key;
    }

    public static string T(string key, params object?[] args)
    {
        return string.Format(CultureInfo.CurrentCulture, T(key), args);
    }

    private static string NormalizeLanguage(string? language)
    {
        var value = string.IsNullOrWhiteSpace(language)
            ? CultureInfo.CurrentUICulture.TwoLetterISOLanguageName
            : language.Trim();

        return value.ToLowerInvariant() switch
        {
            "ru" => "ru",
            "he" or "iw" or "heb" => "he",
            _ => "en",
        };
    }
}
