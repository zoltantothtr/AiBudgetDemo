namespace DemoApp.Application.Transactions.Commands
{
    using DemoApp.Domain.Enums;
    using MediatR;

    public record CreateTransactionCommand(DateTimeOffset OccurredAt, TransactionType Type, Guid CategoryId, string Currency, decimal Amount) : IRequest<Guid>;
}
