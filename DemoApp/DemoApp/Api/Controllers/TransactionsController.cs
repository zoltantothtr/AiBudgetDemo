namespace DemoApp.Api.Controllers
{
    using DemoApp.Application.Transactions.Commands;
    using DemoApp.Application.Transactions.Queries;
    using DemoApp.Domain.Enums;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// Manages transaction resources.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionsController : ControllerBase
    {
        private readonly IMediator mediator;

        public TransactionsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        /// <summary>
        /// Creates a new transaction.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<Guid>> CreateAsync([FromBody] CreateTransactionCommand command, CancellationToken cancellationToken)
        {
            var id = await this.mediator.Send(command, cancellationToken);
            return this.Ok(id);
        }

        /// <summary>
        /// Lists transactions filtered by query parameters.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult> GetListAsync(
            [FromQuery] DateTimeOffset? from,
            [FromQuery] DateTimeOffset? to,
            [FromQuery] TransactionType? type,
            [FromQuery] Guid? categoryId,
            CancellationToken cancellationToken)
        {
            var result = await this.mediator.Send(new GetTransactionsQuery(from, to, type, categoryId), cancellationToken);
            return this.Ok(result);
        }
    }
}
