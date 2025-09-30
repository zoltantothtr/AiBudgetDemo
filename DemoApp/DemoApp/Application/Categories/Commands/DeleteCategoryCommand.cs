namespace DemoApp.Application.Categories.Commands
{
    using MediatR;

    public record DeleteCategoryCommand(Guid Id) : IRequest;
}
