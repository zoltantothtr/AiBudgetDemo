namespace DemoApp.Domain.Entities
{
    using DemoApp.Domain.Enums;

    /// <summary>
    /// Income or expense transaction.
    /// </summary>
    public class Transaction
    {
        public Guid Id { get; set; }

        public DateTimeOffset OccurredAt { get; set; }

        public TransactionType Type { get; set; }

        public Guid CategoryId { get; set; }

        public Category Category { get; set; } = null!;

        public string Currency { get; set; } = "HUF";

        public decimal Amount { get; set; }

        public decimal RateToHuf { get; set; }

        public decimal AmountInHuf { get; set; }
    }
}
