namespace DemoApp.Infrastructure.Services
{
    using System.Net.Http.Json;
    using DemoApp.Application.Abstractions;
    using DemoApp.Domain.Entities;
    using DemoApp.Infrastructure.Persistence;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Caching.Memory;

    /// <summary>
    /// Provides exchange rate retrieval with caching and persistence.
    /// </summary>
    public class ExchangeRateService : IExchangeRateService
    {
        private const string BaseCurrency = "HUF";
        private readonly AppDbContext dbContext;
        private readonly IMemoryCache memoryCache;
        private readonly IHttpClientFactory httpClientFactory;
        private readonly ILogger<ExchangeRateService> logger;

        public ExchangeRateService(AppDbContext dbContext, IMemoryCache memoryCache, IHttpClientFactory httpClientFactory, ILogger<ExchangeRateService> logger)
        {
            this.dbContext = dbContext;
            this.memoryCache = memoryCache;
            this.httpClientFactory = httpClientFactory;
            this.logger = logger;
        }

        /// <inheritdoc />
        public async Task<decimal> GetRateToHufAsync(string currency, DateOnly date, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(currency))
            {
                throw new ArgumentException("Currency required", nameof(currency));
            }

            currency = currency.Trim().ToUpperInvariant();
            if (currency == BaseCurrency)
            {
                return 1m;
            }

            var cacheKey = $"rate:{date:yyyyMMdd}:{currency}";
            if (this.memoryCache.TryGetValue(cacheKey, out decimal cached))
            {
                return cached;
            }

            var entity = await this.dbContext.ExchangeRates.FirstOrDefaultAsync(x => x.Date == date && x.BaseCurrency == BaseCurrency && x.QuoteCurrency == currency, cancellationToken);
            if (entity != null)
            {
                this.memoryCache.Set(cacheKey, entity.Rate, TimeSpan.FromHours(24));
                return entity.Rate;
            }

            var rate = await this.FetchRateAsync(currency, date, cancellationToken);
            entity = new ExchangeRate
            {
                Id = Guid.NewGuid(),
                Date = date,
                BaseCurrency = BaseCurrency,
                QuoteCurrency = currency,
                Rate = rate,
                CreatedAt = DateTimeOffset.UtcNow,
            };
            this.dbContext.ExchangeRates.Add(entity);
            await this.dbContext.SaveChangesAsync(cancellationToken);
            this.memoryCache.Set(cacheKey, rate, TimeSpan.FromHours(24));
            return rate;
        }

        private async Task<decimal> FetchRateAsync(string currency, DateOnly date, CancellationToken cancellationToken)
        {
            var client = this.httpClientFactory.CreateClient("exchangerate");
            var path = date == DateOnly.FromDateTime(DateTime.UtcNow.Date)
                ? "latest"
                : date.ToString("yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);

            var url = $"{path}?from={currency}&to={BaseCurrency}";
            try
            {
                using var response = await client.GetAsync(url, cancellationToken);
                if (!response.IsSuccessStatusCode)
                {
                    this.logger.LogWarning("Exchange rate API status {Status} for {Url}", response.StatusCode, url);
                    throw new InvalidOperationException("Rate API failure");
                }

                var json = await response.Content.ReadFromJsonAsync<FrankfurterResponse>(cancellationToken: cancellationToken);
                if (json?.Rates != null && json.Rates.TryGetValue(BaseCurrency, out var rate) && rate > 0m)
                {
                    return rate;
                }
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Failed to fetch exchange rate for {Currency} on {Date}", currency, date);
            }

            throw new InvalidOperationException($"Could not retrieve exchange rate for {currency} to {BaseCurrency} on {date:yyyy-MM-dd}.");
        }

        private sealed class FrankfurterResponse
        {
            public Dictionary<string, decimal> Rates { get; set; } = new();
        }
    }
}
