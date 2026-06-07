namespace PdfResizer;

public sealed class PdfPreviewBox : Control
{
    private Image? _pageImage;
    private SizeF _pageSizeInches;
    private TrimSettings _trimSettings = new();

    public PdfPreviewBox()
    {
        DoubleBuffered = true;
        BackColor = Color.White;
    }

    public void SetPreview(Image pageImage, SizeF pageSizeInches)
    {
        _pageImage?.Dispose();
        _pageImage = pageImage;
        _pageSizeInches = pageSizeInches;
        Invalidate();
    }

    public void SetTrim(TrimSettings trimSettings)
    {
        _trimSettings = trimSettings;
        Invalidate();
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);

        if (_pageImage == null)
        {
            return;
        }

        var imageRect = GetImageRectangle();
        e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
        e.Graphics.DrawImage(_pageImage, imageRect);

        using var borderPen = new Pen(Color.FromArgb(190, 190, 190), 1);
        e.Graphics.DrawRectangle(borderPen, imageRect);

        var trimRect = GetTrimRectangle(imageRect);
        using var trimPen = new Pen(Color.Red, 3);
        e.Graphics.DrawRectangle(trimPen, trimRect);
    }

    private Rectangle GetImageRectangle()
    {
        if (_pageImage == null)
        {
            return Rectangle.Empty;
        }

        var availableWidth = Math.Max(1, ClientSize.Width - 24);
        var availableHeight = Math.Max(1, ClientSize.Height - 24);
        var scale = Math.Min((float)availableWidth / _pageImage.Width, (float)availableHeight / _pageImage.Height);
        var width = Math.Max(1, (int)(_pageImage.Width * scale));
        var height = Math.Max(1, (int)(_pageImage.Height * scale));
        var x = (ClientSize.Width - width) / 2;
        var y = (ClientSize.Height - height) / 2;
        return new Rectangle(x, y, width, height);
    }

    private Rectangle GetTrimRectangle(Rectangle imageRect)
    {
        var pageWidth = Math.Max(0.01f, _pageSizeInches.Width);
        var pageHeight = Math.Max(0.01f, _pageSizeInches.Height);

        var left = imageRect.Left + imageRect.Width * (_trimSettings.Left / 100f) / pageWidth;
        var top = imageRect.Top + imageRect.Height * (_trimSettings.Top / 100f) / pageHeight;
        var right = imageRect.Right - imageRect.Width * (_trimSettings.Right / 100f) / pageWidth;
        var bottom = imageRect.Bottom - imageRect.Height * (_trimSettings.Bottom / 100f) / pageHeight;

        var width = Math.Max(1, right - left);
        var height = Math.Max(1, bottom - top);
        return Rectangle.Round(new RectangleF(left, top, width, height));
    }
}
