namespace DemoApp.Application.Statistics.Queries
{
    using MediatR;

    public record GetSummaryStatisticsQuery(DateTimeOffset? From, DateTimeOffset? To, string Currency) : IRequest<SummaryStatisticsResult>;
}
