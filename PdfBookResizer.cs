using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Pdf.Xobject;

public static class PdfBookResizer
{
    private const float Inch = 72f;

    public static int GetPageCount(string inputPdf)
    {
        using var reader = new PdfReader(inputPdf);
        using var srcDoc = new PdfDocument(reader);
        return srcDoc.GetNumberOfPages();
    }

    public static void ConvertTo6x9(
        string inputPdf,
        string outputPdf,
        float shift,
        int startPage = 1,
        int? endPage = null)
    {
        ConvertToPageSize(inputPdf, outputPdf, 6f, 9f, shift, startPage, endPage);
    }

    public static void ConvertToPageSize(
        string inputPdf,
        string outputPdf,
        float targetWidthInches,
        float targetHeightInches,
        float shift,
        int startPage = 1,
        int? endPage = null)
    {
        float targetWidth = targetWidthInches * Inch;
        float targetHeight = targetHeightInches * Inch;
        float innerMargin = shift * Inch;

        using var reader = new PdfReader(inputPdf);
        using var writer = new PdfWriter(outputPdf);
        using var srcDoc = new PdfDocument(reader);
        using var dstDoc = new PdfDocument(writer);

        int lastPage = endPage ?? srcDoc.GetNumberOfPages();

        for (int pageNum = startPage; pageNum <= lastPage; pageNum++)
        {
            PdfPage srcPage = srcDoc.GetPage(pageNum);
            iText.Kernel.Geom.Rectangle srcSize = srcPage.GetPageSizeWithRotation();

            float srcW = srcSize.GetWidth();
            float srcH = srcSize.GetHeight();

            float scale = targetHeight / srcH;
            float scaledW = srcW * scale;

            if (scaledW > targetWidth)
            {
                throw new Exception($"Page {pageNum} is too wide after scaling: {scaledW / Inch:0.###} inch");
            }

            float x;
            if (pageNum % 2 == 1)
            {
                x = innerMargin;
            }
            else
            {
                x = targetWidth - scaledW - innerMargin;
            }

            PdfPage newPage = dstDoc.AddNewPage(new PageSize(targetWidth, targetHeight));
            PdfFormXObject pageCopy = srcPage.CopyAsFormXObject(dstDoc);
            PdfCanvas canvas = new PdfCanvas(newPage);

            canvas.AddXObjectWithTransformationMatrix(
                pageCopy,
                scale, 0,
                0, scale,
                x, 0);
        }
    }

    public static void TrimPdf(
        string inputPdf,
        string outputPdf,
        float left,
        float top,
        float right,
        float bottom)
    {
        using var reader = new PdfReader(inputPdf);
        using var writer = new PdfWriter(outputPdf);
        using var srcDoc = new PdfDocument(reader);
        using var dstDoc = new PdfDocument(writer);

        float leftPoints = left * Inch;
        float topPoints = top * Inch;
        float rightPoints = right * Inch;
        float bottomPoints = bottom * Inch;

        for (int pageNum = 1; pageNum <= srcDoc.GetNumberOfPages(); pageNum++)
        {
            PdfPage srcPage = srcDoc.GetPage(pageNum);
            iText.Kernel.Geom.Rectangle srcSize = srcPage.GetPageSizeWithRotation();

            float newWidth = srcSize.GetWidth() - leftPoints - rightPoints;
            float newHeight = srcSize.GetHeight() - topPoints - bottomPoints;

            if (newWidth <= 0 || newHeight <= 0)
            {
                throw new Exception($"Page {pageNum} trim values are larger than the page size.");
            }

            PdfPage newPage = dstDoc.AddNewPage(new PageSize(newWidth, newHeight));
            PdfFormXObject pageCopy = srcPage.CopyAsFormXObject(dstDoc);
            PdfCanvas canvas = new PdfCanvas(newPage);

            canvas.AddXObjectWithTransformationMatrix(
                pageCopy,
                1, 0,
                0, 1,
                -leftPoints, -bottomPoints);
        }
    }
}
