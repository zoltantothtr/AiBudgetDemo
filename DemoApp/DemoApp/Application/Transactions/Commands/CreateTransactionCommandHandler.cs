namespace DemoApp.Application.Transactions.Commands
{
    using DemoApp.Application.Abstractions;
    using DemoApp.Domain.Entities;
    using DemoApp.Infrastructure.Persistence;
    using MediatR;
    using Microsoft.EntityFrameworkCore;

    public class CreateTransactionCommandHandler : IRequestHandler<CreateTransactionCommand, Guid>
    {
        private readonly AppDbContext dbContext;
        private readonly IExchangeRateService exchangeRateService;

        public CreateTransactionCommandHandler(AppDbContext dbContext, IExchangeRateService exchangeRateService)
        {
            this.dbContext = dbContext;
            this.exchangeRateService = exchangeRateService;
        }

        public async Task<Guid> Handle(CreateTransactionCommand request, CancellationToken cancellationToken)
        {
            var categoryExists = await this.dbContext.Categories.AnyAsync(x => x.Id == request.CategoryId, cancellationToken);
            if (!categoryExists)
            {
                throw new InvalidOperationException("Category not found");
            }

            var rate = await this.exchangeRateService.GetRateToHufAsync(request.Currency, DateOnly.FromDateTime(request.OccurredAt.UtcDateTime), cancellationToken);
            var amountInHuf = request.Amount * rate;
            var entity = new Transaction
            {
                Id = Guid.NewGuid(),
                OccurredAt = request.OccurredAt,
                Type = request.Type,
                CategoryId = request.CategoryId,
                Currency = request.Currency,
                Amount = request.Amount,
                RateToHuf = rate,
                AmountInHuf = decimal.Round(amountInHuf, 2),
            };
            this.dbContext.Transactions.Add(entity);
            await this.dbContext.SaveChangesAsync(cancellationToken);
            return entity.Id;
        }
    }
}
