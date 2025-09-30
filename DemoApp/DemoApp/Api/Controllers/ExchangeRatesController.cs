namespace DemoApp.Api.Controllers
{
    using DemoApp.Application.Abstractions;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("api/[controller]")]
    public class ExchangeRatesController : ControllerBase
    {
        private readonly IExchangeRateService exchangeRateService;

        public ExchangeRatesController(IExchangeRateService exchangeRateService)
        {
            this.exchangeRateService = exchangeRateService;
        }

        [HttpGet]
        public async Task<ActionResult> GetAsync([FromQuery] string currency, [FromQuery] DateOnly date, CancellationToken cancellationToken)
        {
            var rate = await this.exchangeRateService.GetRateToHufAsync(currency, date, cancellationToken);
            return this.Ok(new { currency, date, rate });
        }
    }
}
