using System.Drawing.Imaging;
using System.Globalization;
using System.Text;

namespace PdfPageStudio;

public sealed class PdfRasterWriter
{
    private readonly List<PdfRasterPage> _pages = [];

    public void AddPage(Image image, SizeF pageSizePoints)
    {
        using var stream = new MemoryStream();
        SaveJpeg(image, stream);
        _pages.Add(new PdfRasterPage(stream.ToArray(), image.Width, image.Height, pageSizePoints));
    }

    public void Save(string path)
    {
        var objects = new List<byte[]>
        {
            Bytes("<< /Type /Catalog /Pages 2 0 R >>"),
            Array.Empty<byte>(),
        };

        var pageObjectNumbers = new List<int>();
        foreach (var page in _pages)
        {
            var imageObjectNumber = objects.Count + 1;
            objects.Add(BuildImageObject(page));
            var contentObjectNumber = objects.Count + 1;
            objects.Add(BuildContentObject(page, imageObjectNumber));
            var pageObjectNumber = objects.Count + 1;
            objects.Add(BuildPageObject(page, imageObjectNumber, contentObjectNumber));
            pageObjectNumbers.Add(pageObjectNumber);
        }

        objects[1] = BuildPagesObject(pageObjectNumbers);

        using var file = new FileStream(path, FileMode.Create, FileAccess.Write);
        file.Write(Bytes("%PDF-1.4\n"));
        var offsets = new List<long> { 0 };
        for (var index = 0; index < objects.Count; index++)
        {
            offsets.Add(file.Position);
            file.Write(Bytes(Invariant($"{index + 1} 0 obj\n")));
            file.Write(objects[index]);
            file.Write(Bytes("\nendobj\n"));
        }

        var xrefOffset = file.Position;
        file.Write(Bytes(string.Format(CultureInfo.InvariantCulture, "xref\n0 {0}\n", objects.Count + 1)));
        file.Write(Bytes("0000000000 65535 f \n"));
        foreach (var offset in offsets.Skip(1))
        {
            file.Write(Bytes(string.Format(CultureInfo.InvariantCulture, "{0:0000000000} 00000 n \n", offset)));
        }

        file.Write(Bytes(string.Format(CultureInfo.InvariantCulture,
            "trailer\n<< /Size {0} /Root 1 0 R >>\nstartxref\n{1}\n%%EOF\n",
            objects.Count + 1,
            xrefOffset)));
    }

    private static byte[] BuildPagesObject(IReadOnlyList<int> pageObjectNumbers)
    {
        var kids = string.Join(" ", pageObjectNumbers.Select(number => Invariant($"{number} 0 R")));
        return Bytes(string.Format(CultureInfo.InvariantCulture, "<< /Type /Pages /Count {0} /Kids [{1}] >>", pageObjectNumbers.Count, kids));
    }

    private static byte[] BuildImageObject(PdfRasterPage page)
    {
        using var stream = new MemoryStream();
        stream.Write(Bytes(string.Format(CultureInfo.InvariantCulture,
            "<< /Type /XObject /Subtype /Image /Width {0} /Height {1} /ColorSpace /DeviceRGB /BitsPerComponent 8 /Filter /DCTDecode /Length {2} >>\nstream\n",
            page.PixelWidth,
            page.PixelHeight,
            page.ImageBytes.Length)));
        stream.Write(page.ImageBytes);
        stream.Write(Bytes("\nendstream"));
        return stream.ToArray();
    }

    private static byte[] BuildContentObject(PdfRasterPage page, int imageObjectNumber)
    {
        var width = Format(page.PageSizePoints.Width);
        var height = Format(page.PageSizePoints.Height);
        var content = $"q\n{width} 0 0 {height} 0 0 cm\n/Im{imageObjectNumber} Do\nQ\n";
        var contentBytes = Bytes(content);
        using var stream = new MemoryStream();
        stream.Write(Bytes(string.Format(CultureInfo.InvariantCulture, "<< /Length {0} >>\nstream\n", contentBytes.Length)));
        stream.Write(contentBytes);
        stream.Write(Bytes("endstream"));
        return stream.ToArray();
    }

    private static byte[] BuildPageObject(PdfRasterPage page, int imageObjectNumber, int contentObjectNumber)
    {
        return Bytes(string.Format(CultureInfo.InvariantCulture,
            "<< /Type /Page /Parent 2 0 R /MediaBox [0 0 {0} {1}] /Resources << /XObject << /Im{2} {2} 0 R >> >> /Contents {3} 0 R >>",
            Format(page.PageSizePoints.Width),
            Format(page.PageSizePoints.Height),
            imageObjectNumber,
            contentObjectNumber));
    }

    private static void SaveJpeg(Image image, Stream stream)
    {
        var codec = ImageCodecInfo.GetImageEncoders().FirstOrDefault(item => item.FormatID == ImageFormat.Jpeg.Guid);
        if (codec == null)
        {
            image.Save(stream, ImageFormat.Jpeg);
            return;
        }

        using var parameters = new EncoderParameters(1);
        parameters.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 92L);
        image.Save(stream, codec, parameters);
    }

    private static byte[] Bytes(string text)
    {
        return Encoding.ASCII.GetBytes(text);
    }

    private static string Format(float value)
    {
        return value.ToString("0.###", CultureInfo.InvariantCulture);
    }

    private static string Invariant(FormattableString value)
    {
        return FormattableString.Invariant(value);
    }

    private sealed record PdfRasterPage(byte[] ImageBytes, int PixelWidth, int PixelHeight, SizeF PageSizePoints);
}
