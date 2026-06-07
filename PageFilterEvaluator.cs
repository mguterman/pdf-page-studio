namespace PdfPageStudio;

public static class PageFilterEvaluator
{
    public static bool AppliesToPage(PageFilter filter, int pageNumber)
    {
        if (pageNumber < 1)
        {
            return false;
        }

        if (filter.Type == PageFilterType.Odd && pageNumber % 2 == 0)
        {
            return false;
        }

        if (filter.Type == PageFilterType.Even && pageNumber % 2 != 0)
        {
            return false;
        }

        if (filter.Range.Count == 0)
        {
            return true;
        }

        return filter.Range.Any(range =>
            pageNumber >= range.Start &&
            (range.End == null || pageNumber <= range.End.Value));
    }
}
