namespace DemoApp.Api.Controllers
{
    using DemoApp.Application.Statistics.Queries;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("api/[controller]")]
    public class StatisticsController : ControllerBase
    {
        private readonly IMediator mediator;

        public StatisticsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet("summary")]
        public async Task<ActionResult> GetSummaryAsync([FromQuery] DateTimeOffset? from, [FromQuery] DateTimeOffset? to, [FromQuery] string currency, CancellationToken cancellationToken)
        {
            var result = await this.mediator.Send(new GetSummaryStatisticsQuery(from, to, currency), cancellationToken);
            return this.Ok(result);
        }
    }
}
