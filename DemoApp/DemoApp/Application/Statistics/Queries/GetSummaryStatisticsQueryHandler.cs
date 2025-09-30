namespace DemoApp.Application.Statistics.Queries
{
    using DemoApp.Infrastructure.Persistence;
    using MediatR;
    using Microsoft.EntityFrameworkCore;

    public class GetSummaryStatisticsQueryHandler : IRequestHandler<GetSummaryStatisticsQuery, SummaryStatisticsResult>
    {
        private readonly AppDbContext dbContext;

        public GetSummaryStatisticsQueryHandler(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<SummaryStatisticsResult> Handle(GetSummaryStatisticsQuery request, CancellationToken cancellationToken)
        {
            var query = this.dbContext.Transactions.AsNoTracking();
            if (request.From.HasValue)
            {
                query = query.Where(t => t.OccurredAt >= request.From);
            }

            if (request.To.HasValue)
            {
                query = query.Where(t => t.OccurredAt <= request.To);
            }

            if (!string.IsNullOrWhiteSpace(request.Currency))
            {
                query = query.Where(t => t.Currency == request.Currency);
            }

            var totalHuf = await query.SumAsync(t => (decimal?)t.AmountInHuf, cancellationToken) ?? 0m;
            var totalOriginal = await query.SumAsync(t => (decimal?)t.Amount, cancellationToken) ?? 0m;

            var breakdown = await query
                .GroupBy(t => new { t.CategoryId, t.Category.Name, t.Currency })
                .Select(g => new CategoryBreakdownItem(
                    g.Key.CategoryId,
                    g.Key.Name,
                    g.Sum(x => x.AmountInHuf),
                    g.Sum(x => x.Amount),
                    g.Key.Currency))
                .ToListAsync(cancellationToken);

            return new SummaryStatisticsResult(totalHuf, totalOriginal, request.Currency, breakdown);
        }
    }
}
