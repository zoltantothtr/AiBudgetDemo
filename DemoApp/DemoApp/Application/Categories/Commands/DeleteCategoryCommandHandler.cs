namespace DemoApp.Application.Categories.Commands
{
    using DemoApp.Infrastructure.Persistence;
    using MediatR;
    using Microsoft.EntityFrameworkCore;

    public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand>
    {
        private readonly AppDbContext dbContext;

        public DeleteCategoryCommandHandler(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Unit> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            var entity = await this.dbContext.Categories.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
            if (entity == null)
            {
                return Unit.Value;
            }

            var inUse = await this.dbContext.Transactions.AnyAsync(t => t.CategoryId == entity.Id, cancellationToken);
            if (inUse)
            {
                throw new InvalidOperationException("Category in use");
            }

            this.dbContext.Categories.Remove(entity);
            await this.dbContext.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
