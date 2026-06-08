namespace PdfPageStudio;

public sealed class PdfPageStudioProject
{
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    public string PdfFilePath { get; set; } = "";
    public string OutputFolder { get; set; } = "";
    public UnitType UnitType { get; set; } = UnitType.Inch;
    public PreviewApplyMode PreviewApplyMode { get; set; } = PreviewApplyMode.ApplyAll;
    public List<ProjectAction> Actions { get; set; } = [];
}

public sealed class ProjectAction
{
    public string Id { get; set; } = Guid.NewGuid().ToString("N");
    public PdfActionType Type { get; set; } = PdfActionType.Trim;
    public string Name { get; set; } = "Trim";
    public bool? UseDefaultName { get; set; }
    public PageFilter PageFilter { get; set; } = new();
    public float? Left { get; set; }
    public float? Top { get; set; }
    public float? Right { get; set; }
    public float? Bottom { get; set; }
    public float? TargetedWidth { get; set; }
    public float? TargetedHeight { get; set; }
    public bool? Proportional { get; set; }
    public AnchorHorizontal AnchorHorizontal { get; set; } = AnchorHorizontal.Center;
    public AnchorVertical AnchorVertical { get; set; } = AnchorVertical.Center;
    public string Color { get; set; } = "#FF0000";
    public RulerStyle Style { get; set; } = RulerStyle.Solid;
    public RulerValueMode RulerValueMode { get; set; } = RulerValueMode.Percent;
    public RulerOrientation Orientation { get; set; } = RulerOrientation.Vertical;
    public float? Position { get; set; }
}

public sealed class PageFilter
{
    public List<PageFilterRange> Range { get; set; } = [];
    public PageFilterType Type { get; set; } = PageFilterType.Any;
}

public sealed class PageFilterRange
{
    public int Start { get; set; }
    public int? End { get; set; }
}

public enum UnitType
{
    Inch,
    Cm,
}

public enum PdfActionType
{
    Trim,
    Expand,
    Resize,
    Zoom,
    AddRuler,
}

public enum PageFilterType
{
    Any,
    Odd,
    Even,
}

public enum AnchorHorizontal
{
    Left,
    Center,
    Right,
}

public enum AnchorVertical
{
    Top,
    Center,
    Bottom,
}

public enum RulerStyle
{
    Solid,
    Dotted,
}

public enum RulerValueMode
{
    Percent,
    Unit,
}

public enum RulerOrientation
{
    Vertical,
    Horizontal,
}

public enum PreviewApplyMode
{
    ApplyAll,
    UntilCurrent,
}
