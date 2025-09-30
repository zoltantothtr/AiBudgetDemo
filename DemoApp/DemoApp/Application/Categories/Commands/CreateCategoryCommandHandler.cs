namespace DemoApp.Application.Categories.Commands
{
    using DemoApp.Domain.Entities;
    using DemoApp.Infrastructure.Persistence;
    using MediatR;
    using Microsoft.EntityFrameworkCore;

    public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, Guid>
    {
        private readonly AppDbContext dbContext;

        public CreateCategoryCommandHandler(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Guid> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            var exists = await this.dbContext.Categories.AnyAsync(c => c.Name == request.Name, cancellationToken);
            if (exists)
            {
                throw new InvalidOperationException("Category already exists");
            }

            var entity = new Category
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                IsIncome = request.IsIncome,
            };
            this.dbContext.Categories.Add(entity);
            await this.dbContext.SaveChangesAsync(cancellationToken);
            return entity.Id;
        }
    }
}
