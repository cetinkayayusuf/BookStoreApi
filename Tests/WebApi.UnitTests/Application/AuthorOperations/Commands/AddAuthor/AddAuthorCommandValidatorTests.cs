using FluentAssertions;
using TestSetup;
using WebApi.Application.AuthorOperations.Commands.AddAuthor;

namespace Application.AuthorOperations.Commands.AddAuthor
{
    public class AddAuthorCommandValidatorTests : IClassFixture<CommonTestFixture>
    {
        [Theory]
        [InlineData("firstName", "")]
        [InlineData("", "lastName")]
        [InlineData("", "")]
        public void WhenInvalidInputsAreGiven_Validator_ShouldBeReturnError(string firstName, string lastName)
        {
            // arrange
            AddAuthorCommand command = new AddAuthorCommand(null, null);
            command.Model = new AddAuthorModel()
            {
                FirstName = firstName,
                LastName = lastName,
                BirthDate = new DateTime(2004, 4, 24)
            };

            // act 
            AddAuthorCommandValidator validator = new AddAuthorCommandValidator();
            var result = validator.Validate(command);

            // assert
            result.Errors.Count.Should().BeGreaterThan(0);
        }

        [Fact]
        public void WhenDateTimeEqualNowGiven_Validator_ShouldBeReturnError()
        {
            // arrange
            AddAuthorCommand command = new AddAuthorCommand(null, null);
            command.Model = new AddAuthorModel()
            {
                FirstName = "WhenDateTimeEqualNowGiven_Validator_ShouldBeReturnError",
                LastName = "WhenDateTimeEqualNowGiven_Validator_ShouldBeReturnError",
                BirthDate = DateTime.Now.Date
            };

            // act 
            AddAuthorCommandValidator validator = new AddAuthorCommandValidator();
            var result = validator.Validate(command);

            // assert
            result.Errors.Count.Should().BeGreaterThan(0);
        }

        [Fact]
        public void WhenValidInputAreGiven_Validator_ShouldNotBeReturnError()
        {
            // arrange
            AddAuthorCommand command = new AddAuthorCommand(null, null);
            command.Model = new AddAuthorModel()
            {
                FirstName = "WhenValidInputAreGiven_Validator_ShouldNotBeReturnError",
                LastName = "WhenValidInputAreGiven_Validator_ShouldNotBeReturnError",
                BirthDate = DateTime.Now.AddYears(-2)
            };

            // act 
            AddAuthorCommandValidator validator = new AddAuthorCommandValidator();
            var result = validator.Validate(command);

            // assert
            result.Errors.Count.Should().Be(0);
        }
    }
}
