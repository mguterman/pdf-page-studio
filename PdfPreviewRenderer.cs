using PdfiumViewer;

namespace PdfResizer;

public static class PdfPreviewRenderer
{
    public static (Image Image, SizeF PageSizeInches) RenderFirstPage(string pdfPath)
    {
        PdfiumNativeLoader.EnsureLoaded();

        using var document = PdfDocument.Load(pdfPath);
        var pageSize = document.PageSizes[0];
        var pageSizeInches = new SizeF(pageSize.Width / 72f, pageSize.Height / 72f);

        const int targetHeight = 1400;
        var targetWidth = Math.Max(1, (int)(targetHeight * pageSize.Width / pageSize.Height));
        var image = document.Render(0, targetWidth, targetHeight, 144, 144, PdfRenderFlags.Annotations);
        return (image, pageSizeInches);
    }
}
