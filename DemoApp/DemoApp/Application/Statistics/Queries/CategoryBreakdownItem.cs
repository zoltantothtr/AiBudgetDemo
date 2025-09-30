namespace DemoApp.Application.Statistics.Queries
{
    public record CategoryBreakdownItem(Guid CategoryId, string CategoryName, decimal TotalAmountHuf, decimal TotalOriginalAmount, string Currency);
}
