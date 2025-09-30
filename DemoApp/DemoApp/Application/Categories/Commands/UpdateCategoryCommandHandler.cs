namespace DemoApp.Application.Categories.Commands
{
    using DemoApp.Infrastructure.Persistence;
    using MediatR;
    using Microsoft.EntityFrameworkCore;

    public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand>
    {
        private readonly AppDbContext dbContext;

        public UpdateCategoryCommandHandler(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Unit> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            var entity = await this.dbContext.Categories.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
            if (entity == null)
            {
                throw new InvalidOperationException("Category not found");
            }

            entity.Name = request.Name;
            entity.IsIncome = request.IsIncome;
            await this.dbContext.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
