using System.Reflection;
using System.Runtime.InteropServices;

namespace PdfPageStudio;

internal static class PdfiumNativeLoader
{
    private const string DllName = "pdfium.dll";

    public static void EnsureLoaded()
    {
        var targetPath = Path.Combine(AppContext.BaseDirectory, DllName);
        if (!File.Exists(targetPath))
        {
            using var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(DllName)
                ?? throw new FileNotFoundException("Embedded pdfium.dll resource was not found.");
            using var file = File.Create(targetPath);
            stream.CopyTo(file);
        }

        NativeLibrary.Load(targetPath);
    }
}
