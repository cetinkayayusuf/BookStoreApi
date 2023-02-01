using FluentValidation;

namespace WebApi.Application.GenreOperations.Commands.AddGenre
{
    public class AddGenreCommandValidator : AbstractValidator<AddGenreCommand>
    {
        public AddGenreCommandValidator()
        {
            RuleFor(command => command.Model.Name).NotEmpty().MinimumLength(2);
        }
    }
}