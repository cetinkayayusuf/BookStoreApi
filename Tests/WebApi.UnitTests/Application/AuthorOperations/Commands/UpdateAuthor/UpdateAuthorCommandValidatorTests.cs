using FluentAssertions;
using TestSetup;
using WebApi.Application.AuthorOperations.Commands.UpdateAuthor;

namespace Application.AuthorOperations.Commands.UpdateAuthor
{
    public class UpdateAuthorCommandValidatorTests : IClassFixture<CommonTestFixture>
    {
        [Theory]
        [InlineData(1, "aaaa", "")]
        [InlineData(1, "", "aaaa")]
        [InlineData(1, "", "")]
        [InlineData(-1, "aaaa", "")]
        [InlineData(-1, "", "aaaa")]
        [InlineData(-1, "aaaa", "aaaa")]
        [InlineData(-1, "", "")]
        public void WhenInvalidInputsAreGiven_Validator_ShouldBeReturnError(int authorId, string firstName, string lastName)
        {
            // arrange
            UpdateAuthorCommand command = new UpdateAuthorCommand(null);
            command.AuthorId = authorId;
            command.Model = new UpdateAuthorModel()
            {
                FirstName = firstName,
                LastName = lastName,
                BirthDate = DateTime.Now.AddYears(-20),
            };

            // act 
            UpdateAuthorCommandValidator validator = new UpdateAuthorCommandValidator();
            var result = validator.Validate(command);

            // assert
            result.Errors.Count.Should().BeGreaterThan(0);
        }

        [Fact]
        public void WhenDateTimeEqualNowGiven_Validator_ShouldBeReturnError()
        {
            // arrange
            UpdateAuthorCommand command = new UpdateAuthorCommand(null);
            command.AuthorId = 1;
            command.Model = new UpdateAuthorModel()
            {
                FirstName = "WhenDateTimeEqualNowGiven_Validator_ShouldBeReturnError",
                LastName = "WhenDateTimeEqualNowGiven_Validator_ShouldBeReturnError",
                BirthDate = DateTime.Now.Date,
            };

            // act 
            UpdateAuthorCommandValidator validator = new UpdateAuthorCommandValidator();
            var result = validator.Validate(command);

            // assert
            result.Errors.Count.Should().BeGreaterThan(0);
        }

        [Fact]
        public void WhenValidInputAreGiven_Validator_ShouldNotBeReturnError()
        {
            // arrange
            UpdateAuthorCommand command = new UpdateAuthorCommand(null);
            command.AuthorId = 1;
            command.Model = new UpdateAuthorModel()
            {
                FirstName = "WhenValidInputAreGiven_Validator_ShouldNotBeReturnError",
                LastName = "WhenValidInputAreGiven_Validator_ShouldNotBeReturnError",
                BirthDate = DateTime.Now.AddYears(-20),
            };

            // act 
            UpdateAuthorCommandValidator validator = new UpdateAuthorCommandValidator();
            var result = validator.Validate(command);

            // assert
            result.Errors.Count.Should().Be(0);
        }
    }
}
