namespace DemoApp.Application.Categories.Queries
{
    using DemoApp.Application.DTOs;
    using MediatR;

    public record GetCategoryByIdQuery(Guid Id) : IRequest<CategoryDto?>;
}
