using FluentAssertions;
using TestSetup;
using WebApi.Application.GenreOperations.Commands.AddGenre;

namespace Application.GenreOperations.Commands.AddGenre
{
    public class AddGenreCommandValidatorTests : IClassFixture<CommonTestFixture>
    {
        [Theory]
        [InlineData("")]
        [InlineData(default)]
        public void WhenInvalidInputsAreGiven_Validator_ShouldBeReturnError(string name)
        {
            // arrange
            AddGenreCommand command = new AddGenreCommand(null, null);
            command.Model = new AddGenreModel()
            {
                Name = name,
            };

            // act 
            AddGenreCommandValidator validator = new AddGenreCommandValidator();
            var result = validator.Validate(command);

            // assert
            result.Errors.Count.Should().BeGreaterThan(0);
        }

        [Fact]
        public void WhenValidInputAreGiven_Validator_ShouldNotBeReturnError()
        {
            // arrange
            AddGenreCommand command = new AddGenreCommand(null, null);
            command.Model = new AddGenreModel()
            {
                Name = "WhenValidInputAreGiven_Validator_ShouldNotBeReturnError"
            };

            // act 
            AddGenreCommandValidator validator = new AddGenreCommandValidator();
            var result = validator.Validate(command);

            // assert
            result.Errors.Count.Should().Be(0);
        }
    }
}
