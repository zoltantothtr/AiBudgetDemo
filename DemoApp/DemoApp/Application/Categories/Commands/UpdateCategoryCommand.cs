namespace DemoApp.Application.Categories.Commands
{
    using MediatR;

    public record UpdateCategoryCommand(Guid Id, string Name, bool IsIncome) : IRequest;
}
