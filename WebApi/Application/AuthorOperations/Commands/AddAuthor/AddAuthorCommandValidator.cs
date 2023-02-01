using FluentValidation;

namespace WebApi.Application.AuthorOperations.Commands.AddAuthor
{
    public class AddAuthorCommandValidator : AbstractValidator<AddAuthorCommand>
    {
        public AddAuthorCommandValidator()
        {
            RuleFor(command => command.Model.FirstName).NotEmpty().MinimumLength(2);
            RuleFor(command => command.Model.LastName).NotEmpty().MinimumLength(2);
            RuleFor(command => command.Model.BirthDate).NotEmpty().LessThan(DateTime.Now.Date); ;
        }
    }
}