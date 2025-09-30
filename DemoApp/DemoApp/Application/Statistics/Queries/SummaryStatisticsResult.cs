namespace DemoApp.Application.Statistics.Queries
{
    public record SummaryStatisticsResult(decimal TotalHuf, decimal TotalOriginal, string Currency, IReadOnlyCollection<CategoryBreakdownItem> CategoryBreakdown);
}
