namespace DemoApp.Application.Transactions.Queries
{
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using DemoApp.Application.DTOs;
    using DemoApp.Infrastructure.Persistence;
    using MediatR;
    using Microsoft.EntityFrameworkCore;

    public class GetTransactionsQueryHandler : IRequestHandler<GetTransactionsQuery, IReadOnlyCollection<TransactionDto>>
    {
        private readonly AppDbContext dbContext;
        private readonly IMapper mapper;

        public GetTransactionsQueryHandler(AppDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        public async Task<IReadOnlyCollection<TransactionDto>> Handle(GetTransactionsQuery request, CancellationToken cancellationToken)
        {
            var query = this.dbContext.Transactions.AsNoTracking();
            if (request.From.HasValue)
            {
                query = query.Where(x => x.OccurredAt >= request.From);
            }

            if (request.To.HasValue)
            {
                query = query.Where(x => x.OccurredAt <= request.To);
            }

            if (request.Type.HasValue)
            {
                query = query.Where(x => x.Type == request.Type);
            }

            if (request.CategoryId.HasValue)
            {
                query = query.Where(x => x.CategoryId == request.CategoryId);
            }

            var list = await query.ProjectTo<TransactionDto>(this.mapper.ConfigurationProvider).ToListAsync(cancellationToken);
            return list;
        }
    }
}
