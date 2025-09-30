namespace DemoApp.Application.Transactions.Queries
{
    using DemoApp.Application.DTOs;
    using DemoApp.Domain.Enums;
    using MediatR;

    public record GetTransactionsQuery(DateTimeOffset? From, DateTimeOffset? To, TransactionType? Type, Guid? CategoryId) : IRequest<IReadOnlyCollection<TransactionDto>>;
}
