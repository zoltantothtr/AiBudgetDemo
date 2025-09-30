namespace DemoApp.Application.Abstractions
{
    /// <summary>
    /// Provides access to exchange rates for converting currencies to HUF.
    /// </summary>
    public interface IExchangeRateService
    {
        /// <summary>
        /// Gets conversion rate from provided currency to HUF for given date.
        /// </summary>
        /// <param name="currency">Currency code (e.g. EUR).</param>
        /// <param name="date">Date of rate.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Rate to multiply original amount to get HUF amount.</returns>
        Task<decimal> GetRateToHufAsync(string currency, DateOnly date, CancellationToken cancellationToken);
    }
}
