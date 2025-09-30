namespace DemoApp.Api.Controllers
{
    using DemoApp.Application.Categories.Commands;
    using DemoApp.Application.Categories.Queries;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly IMediator mediator;

        public CategoriesController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult> GetAsync(CancellationToken cancellationToken)
        {
            var result = await this.mediator.Send(new GetCategoriesQuery(), cancellationToken);
            return this.Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var result = await this.mediator.Send(new GetCategoryByIdQuery(id), cancellationToken);
            if (result == null)
            {
                return this.NotFound();
            }

            return this.Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<Guid>> CreateAsync([FromBody] CreateCategoryCommand command, CancellationToken cancellationToken)
        {
            var id = await this.mediator.Send(command, cancellationToken);
            return this.Ok(id);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateAsync(Guid id, [FromBody] UpdateCategoryCommand body, CancellationToken cancellationToken)
        {
            if (id != body.Id)
            {
                return this.BadRequest();
            }

            await this.mediator.Send(body, cancellationToken);
            return this.NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            await this.mediator.Send(new DeleteCategoryCommand(id), cancellationToken);
            return this.NoContent();
        }
    }
}
