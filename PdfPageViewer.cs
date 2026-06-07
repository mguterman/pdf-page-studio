namespace PdfPageStudio;

public sealed class PdfPageViewer : ScrollableControl
{
    private Image? _pageImage;
    private PdfZoomMode _zoomMode = PdfZoomMode.FitPage;
    private float _customZoom = 1f;
    private Size _lastAutoScrollMinSize;

    public PdfPageViewer()
    {
        AutoScroll = true;
        BackColor = Color.FromArgb(245, 245, 245);
        DoubleBuffered = true;
    }

    public PdfZoomMode ZoomMode
    {
        get => _zoomMode;
        set
        {
            _zoomMode = value;
            UpdateScrollSize();
            Invalidate();
        }
    }

    public float CustomZoom
    {
        get => _customZoom;
        set
        {
            _customZoom = Math.Clamp(value, 0.1f, 6f);
            _zoomMode = PdfZoomMode.Custom;
            UpdateScrollSize();
            Invalidate();
        }
    }

    public void SetPage(Image? image)
    {
        var oldImage = _pageImage;
        _pageImage = image;
        oldImage?.Dispose();
        AutoScrollPosition = Point.Empty;
        UpdateScrollSize();
        Invalidate();
    }

    protected override void OnSizeChanged(EventArgs e)
    {
        base.OnSizeChanged(e);
        if (_zoomMode is PdfZoomMode.FitPage or PdfZoomMode.FitWidth)
        {
            AutoScrollPosition = Point.Empty;
        }

        UpdateScrollSize();
        Invalidate();
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);

        if (_pageImage == null)
        {
            using var brush = new SolidBrush(Color.White);
            e.Graphics.FillRectangle(brush, ClientRectangle);
            return;
        }

        var scale = GetScale();
        var pageSize = GetScaledPageSize(scale);
        var offset = AutoScrollPosition;
        var x = Math.Max((ClientSize.Width - pageSize.Width) / 2, 0) + offset.X;
        var y = Math.Max((ClientSize.Height - pageSize.Height) / 2, 0) + offset.Y;
        var destination = new Rectangle(x, y, pageSize.Width, pageSize.Height);

        e.Graphics.Clear(BackColor);
        e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
        e.Graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;

        using var shadowBrush = new SolidBrush(Color.FromArgb(45, 0, 0, 0));
        e.Graphics.FillRectangle(shadowBrush, destination.X + 4, destination.Y + 4, destination.Width, destination.Height);
        e.Graphics.DrawImage(_pageImage, destination);
        e.Graphics.DrawRectangle(Pens.Gainsboro, destination);
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _pageImage?.Dispose();
        }

        base.Dispose(disposing);
    }

    private void UpdateScrollSize()
    {
        if (_pageImage == null)
        {
            SetAutoScrollMinSize(Size.Empty);
            return;
        }

        SetAutoScrollMinSize(GetScaledPageSize(GetScale()));
    }

    private void SetAutoScrollMinSize(Size size)
    {
        if (_lastAutoScrollMinSize == size)
        {
            return;
        }

        _lastAutoScrollMinSize = size;
        AutoScrollMinSize = size;
    }

    private Size GetScaledPageSize(float scale)
    {
        if (_pageImage == null)
        {
            return Size.Empty;
        }

        return new Size(
            Math.Max(1, (int)Math.Round(_pageImage.Width * scale)),
            Math.Max(1, (int)Math.Round(_pageImage.Height * scale)));
    }

    private float GetScale()
    {
        if (_pageImage == null)
        {
            return 1f;
        }

        var widthScale = Math.Max(0.01f, (ClientSize.Width - 32f) / _pageImage.Width);
        var heightScale = Math.Max(0.01f, (ClientSize.Height - 32f) / _pageImage.Height);

        return _zoomMode switch
        {
            PdfZoomMode.FitWidth => widthScale,
            PdfZoomMode.FitPage => Math.Min(widthScale, heightScale),
            _ => _customZoom,
        };
    }
}
