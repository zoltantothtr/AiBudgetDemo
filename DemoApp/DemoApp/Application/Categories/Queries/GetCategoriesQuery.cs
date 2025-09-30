namespace DemoApp.Application.Categories.Queries
{
    using DemoApp.Application.DTOs;
    using MediatR;

    public record GetCategoriesQuery() : IRequest<IReadOnlyCollection<CategoryDto>>;
}
