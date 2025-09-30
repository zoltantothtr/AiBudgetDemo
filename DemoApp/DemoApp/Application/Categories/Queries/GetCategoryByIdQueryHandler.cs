namespace DemoApp.Application.Categories.Queries
{
    using AutoMapper;
    using DemoApp.Application.DTOs;
    using DemoApp.Infrastructure.Persistence;
    using MediatR;
    using Microsoft.EntityFrameworkCore;

    public class GetCategoryByIdQueryHandler : IRequestHandler<GetCategoryByIdQuery, CategoryDto?>
    {
        private readonly AppDbContext dbContext;
        private readonly IMapper mapper;

        public GetCategoryByIdQueryHandler(AppDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        public async Task<CategoryDto?> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
        {
            var entity = await this.dbContext.Categories.AsNoTracking().FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);
            return entity == null ? null : this.mapper.Map<CategoryDto>(entity);
        }
    }
}
