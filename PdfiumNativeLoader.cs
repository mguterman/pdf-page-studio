using System.Reflection;
using System.Runtime.InteropServices;

namespace PdfResizer;

public static class PdfiumNativeLoader
{
    private static bool _loaded;

    public static void EnsureLoaded()
    {
        if (_loaded)
        {
            return;
        }

        var nativeDirectory = AppContext.BaseDirectory;
        var nativePath = Path.Combine(nativeDirectory, "pdfium.dll");

        Directory.CreateDirectory(nativeDirectory);

        if (!File.Exists(nativePath))
        {
            using var resourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("pdfium.dll")
                ?? throw new FileNotFoundException("Embedded pdfium.dll resource was not found.");
            using var fileStream = File.Create(nativePath);
            resourceStream.CopyTo(fileStream);
        }

        if (LoadLibrary(nativePath) == IntPtr.Zero)
        {
            throw new InvalidOperationException("Could not load pdfium.dll from " + nativePath);
        }

        _loaded = true;
    }

    [DllImport("kernel32", SetLastError = true, CharSet = CharSet.Unicode)]
    private static extern IntPtr LoadLibrary(string lpFileName);
}
