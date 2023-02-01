using FluentValidation;

namespace WebApi.Application.AuthorOperations.Commands.UpdateAuthor
{
    public class UpdateAuthorCommandValidator : AbstractValidator<UpdateAuthorCommand>
    {
        public UpdateAuthorCommandValidator()
        {
            RuleFor(command => command.Model.FirstName).MinimumLength(2);
            RuleFor(command => command.Model.LastName).MinimumLength(2);
            RuleFor(command => command.Model.BirthDate).LessThan(DateTime.Now.Date); ;
        }
    }
}