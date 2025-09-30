namespace DemoApp.Application.Categories.Commands
{
    using MediatR;

    public record CreateCategoryCommand(string Name, bool IsIncome) : IRequest<Guid>;
}
