namespace PdfPageStudio;

public sealed class PdfPageStudioProject
{
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    public string PdfFilePath { get; set; } = "";
    public List<ProjectAction> Actions { get; set; } = [];
}

public sealed class ProjectAction
{
    public string Id { get; set; } = Guid.NewGuid().ToString("N");
    public string Type { get; set; } = "NewAction";
    public string Name { get; set; } = "New Action";
    public PageFilter PageFilter { get; set; } = new();
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

public enum PageFilterType
{
    Any,
    Odd,
    Even,
}
