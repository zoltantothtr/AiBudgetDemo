namespace DemoApp.Application.DTOs
{
    public class ExchangeRateDto
    {
        public DateOnly Date { get; set; }

        public string BaseCurrency { get; set; } = string.Empty;

        public string QuoteCurrency { get; set; } = string.Empty;

        public decimal Rate { get; set; }
    }
}
