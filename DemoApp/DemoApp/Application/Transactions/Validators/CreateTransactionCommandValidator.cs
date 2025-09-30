namespace DemoApp.Application.Transactions.Validators
{
    using DemoApp.Application.Transactions.Commands;
    using FluentValidation;

    /// <summary>
    /// Validator for <see cref="CreateTransactionCommand"/>.
    /// </summary>
    public class CreateTransactionCommandValidator : AbstractValidator<CreateTransactionCommand>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateTransactionCommandValidator"/> class.
        /// </summary>
        public CreateTransactionCommandValidator()
        {
            this.RuleFor(x => x.Amount).GreaterThan(0);
            this.RuleFor(x => x.Currency).NotEmpty().MaximumLength(10);
        }
    }
}
