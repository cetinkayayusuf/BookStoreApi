using FluentAssertions;
using TestSetup;
using WebApi.Application.AuthorOperations.Commands.DeleteAuthor;

namespace Application.AuthorOperations.Commands.DeleteAuthor
{
    public class DeleteAuthorCommandValidatorTests : IClassFixture<CommonTestFixture>
    {
        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        public void WhenInvalidInputsAreGiven_Validator_ShouldBeReturnError(int authorId)
        {
            // arrange
            DeleteAuthorCommand command = new DeleteAuthorCommand(null);
            command.AuthorId = authorId;

            // act 
            DeleteAuthorCommandValidator validator = new DeleteAuthorCommandValidator();
            var result = validator.Validate(command);

            // assert
            result.Errors.Count.Should().BeGreaterThan(0);
        }

        [Fact]
        public void WhenValidInputAreGiven_Validator_ShouldNotBeReturnError()
        {
            // arrange
            DeleteAuthorCommand command = new DeleteAuthorCommand(null);
            command.AuthorId = 1;

            // act 
            DeleteAuthorCommandValidator validator = new DeleteAuthorCommandValidator();
            var result = validator.Validate(command);

            // assert
            result.Errors.Count.Should().Be(0);
        }
    }
}
