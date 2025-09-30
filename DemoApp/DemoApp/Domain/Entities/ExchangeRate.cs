namespace DemoApp.Domain.Entities
{
    /// <summary>
    /// Cached exchange rate for a currency to HUF for a specific date.
    /// </summary>
    public class ExchangeRate
    {
        public Guid Id { get; set; }

        public DateOnly Date { get; set; }

        public string BaseCurrency { get; set; } = "HUF";

        public string QuoteCurrency { get; set; } = string.Empty;

        public decimal Rate { get; set; }

        public DateTimeOffset CreatedAt { get; set; }
    }
}
