namespace DemoApp.Application.DTOs
{
    using DemoApp.Domain.Enums;

    public class TransactionDto
    {
        public Guid Id { get; set; }

        public DateTimeOffset OccurredAt { get; set; }

        public TransactionType Type { get; set; }

        public Guid CategoryId { get; set; }

        public string Currency { get; set; } = string.Empty;

        public decimal Amount { get; set; }

        public decimal RateToHuf { get; set; }

        public decimal AmountInHuf { get; set; }
    }
}
