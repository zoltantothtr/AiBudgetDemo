namespace DemoApp.Application.Categories.Queries
{
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using DemoApp.Application.DTOs;
    using DemoApp.Infrastructure.Persistence;
    using MediatR;
    using Microsoft.EntityFrameworkCore;

    public class GetCategoriesQueryHandler : IRequestHandler<GetCategoriesQuery, IReadOnlyCollection<CategoryDto>>
    {
        private readonly AppDbContext dbContext;
        private readonly IMapper mapper;

        public GetCategoriesQueryHandler(AppDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        public async Task<IReadOnlyCollection<CategoryDto>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
        {
            var list = await this.dbContext.Categories.AsNoTracking().OrderBy(c => c.Name).ProjectTo<CategoryDto>(this.mapper.ConfigurationProvider).ToListAsync(cancellationToken);
            return list;
        }
    }
}
