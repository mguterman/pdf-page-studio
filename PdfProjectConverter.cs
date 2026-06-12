using iText.Kernel.Colors;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Pdf.Xobject;

namespace PdfPageStudio;

public static class PdfProjectConverter
{
    private const float PointsPerInch = 72f;
    private const float MinimumPageSize = 1f;

    public static void Convert(string sourcePdfPath, string destinationPath, IReadOnlyList<ProjectAction> actions, UnitType unitType)
    {
        var folder = System.IO.Path.GetDirectoryName(destinationPath);
        if (!string.IsNullOrWhiteSpace(folder))
        {
            Directory.CreateDirectory(folder);
        }

        using var reader = new PdfReader(sourcePdfPath);
        using var writer = new PdfWriter(destinationPath);
        using var sourceDocument = new PdfDocument(reader);
        using var destinationDocument = new PdfDocument(writer);

        for (var pageNumber = 1; pageNumber <= sourceDocument.GetNumberOfPages(); pageNumber++)
        {
            var sourcePage = sourceDocument.GetPage(pageNumber);
            var sourceSize = sourcePage.GetPageSizeWithRotation();
            var pagePlan = BuildPagePlan(sourceSize.GetWidth(), sourceSize.GetHeight(), actions, unitType, pageNumber);

            var destinationPage = destinationDocument.AddNewPage(new PageSize(pagePlan.Width, pagePlan.Height));
            var pageCopy = sourcePage.CopyAsFormXObject(destinationDocument);
            var canvas = new PdfCanvas(destinationPage);
            canvas.AddXObjectWithTransformationMatrix(
                pageCopy,
                pagePlan.ScaleX, 0,
                0, pagePlan.ScaleY,
                pagePlan.TranslateX,
                pagePlan.TranslateY);
            DrawFrames(canvas, pagePlan, actions, unitType, pageNumber);
        }
    }

    private static PagePlan BuildPagePlan(float sourceWidth, float sourceHeight, IReadOnlyList<ProjectAction> actions, UnitType unitType, int pageNumber)
    {
        var plan = new PagePlan(sourceWidth, sourceHeight);
        foreach (var action in actions)
        {
            if (!action.Enabled ||
                !PageFilterEvaluator.AppliesToPage(action.PageFilter, pageNumber) ||
                action.Type is PdfActionType.AddRuler or PdfActionType.AddFrame)
            {
                continue;
            }

            ApplyAction(plan, action, unitType, pageNumber);
        }

        return plan;
    }

    private static void ApplyAction(PagePlan plan, ProjectAction action, UnitType unitType, int pageNumber)
    {
        switch (action.Type)
        {
            case PdfActionType.Trim:
                ApplyTrim(plan, action, unitType, pageNumber);
                break;
            case PdfActionType.Expand:
                ApplyExpand(plan, action, unitType);
                break;
            case PdfActionType.Resize:
                ApplyResize(plan, action, unitType);
                break;
            case PdfActionType.Zoom:
                ApplyZoom(plan, action, unitType);
                break;
            case PdfActionType.AdjustSize:
                ApplyAdjustSize(plan, action, unitType);
                break;
        }
    }

    private static void DrawFrames(PdfCanvas canvas, PagePlan pagePlan, IReadOnlyList<ProjectAction> actions, UnitType unitType, int pageNumber)
    {
        foreach (var action in actions)
        {
            if (!action.Enabled || action.Type != PdfActionType.AddFrame || !PageFilterEvaluator.AppliesToPage(action.PageFilter, pageNumber))
            {
                continue;
            }

            var left = UnitValueToPoints(action.Left, unitType);
            var top = UnitValueToPoints(action.Top, unitType);
            var right = UnitValueToPoints(action.Right, unitType);
            var bottom = UnitValueToPoints(action.Bottom, unitType);
            var width = pagePlan.Width - left - right;
            var height = pagePlan.Height - top - bottom;
            if (width <= 0 || height <= 0)
            {
                continue;
            }

            canvas.SaveState();
            canvas.SetStrokeColor(ParsePdfColor(action.Color));
            canvas.SetLineWidth(1.5f);
            if (action.Style == RulerStyle.Dotted)
            {
                canvas.SetLineDash(2f, 3f);
            }

            canvas.Rectangle(left, bottom, width, height);
            canvas.Stroke();
            canvas.RestoreState();
        }
    }

    private static DeviceRgb ParsePdfColor(string color)
    {
        try
        {
            var drawingColor = ColorTranslator.FromHtml(string.IsNullOrWhiteSpace(color) ? "#FF0000" : color);
            return new DeviceRgb(drawingColor.R, drawingColor.G, drawingColor.B);
        }
        catch
        {
            return new DeviceRgb(255, 0, 0);
        }
    }

    private static void ApplyTrim(PagePlan plan, ProjectAction action, UnitType unitType, int pageNumber)
    {
        var left = UnitValueToPoints(action.Left, unitType);
        var top = UnitValueToPoints(action.Top, unitType);
        var right = UnitValueToPoints(action.Right, unitType);
        var bottom = UnitValueToPoints(action.Bottom, unitType);
        var newWidth = plan.Width - left - right;
        var newHeight = plan.Height - top - bottom;
        if (newWidth <= MinimumPageSize || newHeight <= MinimumPageSize)
        {
            throw new InvalidOperationException($"Page {pageNumber} trim values are larger than the page size.");
        }

        plan.TranslateX -= left;
        plan.TranslateY -= bottom;
        plan.Width = newWidth;
        plan.Height = newHeight;
    }

    private static void ApplyExpand(PagePlan plan, ProjectAction action, UnitType unitType)
    {
        var left = UnitValueToPoints(action.Left, unitType);
        var top = UnitValueToPoints(action.Top, unitType);
        var right = UnitValueToPoints(action.Right, unitType);
        var bottom = UnitValueToPoints(action.Bottom, unitType);

        plan.TranslateX += left;
        plan.TranslateY += bottom;
        plan.Width = Math.Max(MinimumPageSize, plan.Width + left + right);
        plan.Height = Math.Max(MinimumPageSize, plan.Height + top + bottom);
    }

    private static void ApplyResize(PagePlan plan, ProjectAction action, UnitType unitType)
    {
        var targetWidth = action.TargetedWidth is > 0 ? UnitValueToPoints(action.TargetedWidth.Value, unitType) : (float?)null;
        var targetHeight = action.TargetedHeight is > 0 ? UnitValueToPoints(action.TargetedHeight.Value, unitType) : (float?)null;
        if ((targetWidth == null || targetWidth <= 0) && (targetHeight == null || targetHeight <= 0))
        {
            return;
        }

        if (action.Proportional != false)
        {
            if ((targetWidth == null || targetWidth <= 0) && targetHeight > 0)
            {
                targetWidth = plan.Width * targetHeight.Value / plan.Height;
            }
            else if ((targetHeight == null || targetHeight <= 0) && targetWidth > 0)
            {
                targetHeight = plan.Height * targetWidth.Value / plan.Width;
            }
        }

        targetWidth = targetWidth is > 0 ? targetWidth : plan.Width;
        targetHeight = targetHeight is > 0 ? targetHeight : plan.Height;
        var scaleX = targetWidth.Value / plan.Width;
        var scaleY = targetHeight.Value / plan.Height;
        ScaleAboutBottomLeft(plan, scaleX, scaleY, 0, 0);
        plan.Width = Math.Max(MinimumPageSize, targetWidth.Value);
        plan.Height = Math.Max(MinimumPageSize, targetHeight.Value);
    }

    private static void ApplyZoom(PagePlan plan, ProjectAction action, UnitType unitType)
    {
        var widthPercent = action.EnableWidth ? action.TargetedWidth : null;
        var heightPercent = action.EnableHeight ? action.TargetedHeight : null;
        var shiftX = action.EnableShiftX ? action.ShiftX ?? 0 : 0;
        var shiftY = action.EnableShiftY ? action.ShiftY ?? 0 : 0;
        if ((widthPercent == null || widthPercent <= 0) && (heightPercent == null || heightPercent <= 0) &&
            Math.Abs(shiftX) < 0.0001f && Math.Abs(shiftY) < 0.0001f)
        {
            return;
        }

        if (action.Proportional != false && !(widthPercent is > 0 && heightPercent is > 0))
        {
            if ((widthPercent == null || widthPercent <= 0) && heightPercent > 0)
            {
                widthPercent = heightPercent;
            }
            else if ((heightPercent == null || heightPercent <= 0) && widthPercent > 0)
            {
                heightPercent = widthPercent;
            }
        }

        var scaleX = Math.Max(0.001f, (widthPercent ?? 100f) / 100f);
        var scaleY = Math.Max(0.001f, (heightPercent ?? 100f) / 100f);
        var contentWidth = plan.Width * scaleX;
        var contentHeight = plan.Height * scaleY;
        var offsetX = GetAnchorOffset(plan.Width, contentWidth, action.AnchorHorizontal);
        var offsetY = GetAnchorOffset(plan.Height, contentHeight, action.AnchorVertical);
        offsetX += UnitValueToPoints(shiftX, unitType);
        offsetY -= UnitValueToPoints(shiftY, unitType);
        ScaleAboutBottomLeft(plan, scaleX, scaleY, offsetX, offsetY);
    }

    private static void ApplyAdjustSize(PagePlan plan, ProjectAction action, UnitType unitType)
    {
        var targetWidth = action.EnableWidth && action.TargetedWidth is > 0
            ? UnitValueToPoints(action.TargetedWidth.Value, unitType)
            : plan.Width;
        var targetHeight = action.EnableHeight && action.TargetedHeight is > 0
            ? UnitValueToPoints(action.TargetedHeight.Value, unitType)
            : plan.Height;
        var addWidth = targetWidth - plan.Width;
        var addHeight = targetHeight - plan.Height;
        if (Math.Abs(addWidth) < 0.001f && Math.Abs(addHeight) < 0.001f)
        {
            return;
        }

        plan.TranslateX += addWidth / 2f;
        plan.TranslateY += addHeight / 2f;
        plan.Width = Math.Max(MinimumPageSize, targetWidth);
        plan.Height = Math.Max(MinimumPageSize, targetHeight);
    }

    private static void ScaleAboutBottomLeft(PagePlan plan, float scaleX, float scaleY, float offsetX, float offsetY)
    {
        plan.ScaleX *= scaleX;
        plan.ScaleY *= scaleY;
        plan.TranslateX = plan.TranslateX * scaleX + offsetX;
        plan.TranslateY = plan.TranslateY * scaleY + offsetY;
    }

    private static float GetAnchorOffset(float canvasSize, float contentSize, AnchorHorizontal anchor)
    {
        return anchor switch
        {
            AnchorHorizontal.Left => 0,
            AnchorHorizontal.Right => canvasSize - contentSize,
            _ => (canvasSize - contentSize) / 2f,
        };
    }

    private static float GetAnchorOffset(float canvasSize, float contentSize, AnchorVertical anchor)
    {
        return anchor switch
        {
            AnchorVertical.Bottom => 0,
            AnchorVertical.Top => canvasSize - contentSize,
            _ => (canvasSize - contentSize) / 2f,
        };
    }

    private static float UnitValueToPoints(float? value, UnitType unitType)
    {
        return UnitValueToPoints(value ?? 0, unitType);
    }

    private static float UnitValueToPoints(float value, UnitType unitType)
    {
        return (unitType == UnitType.Cm ? value / 2.54f : value) * PointsPerInch;
    }

    private sealed class PagePlan(float width, float height)
    {
        public float Width { get; set; } = width;
        public float Height { get; set; } = height;
        public float ScaleX { get; set; } = 1f;
        public float ScaleY { get; set; } = 1f;
        public float TranslateX { get; set; }
        public float TranslateY { get; set; }
    }
}
