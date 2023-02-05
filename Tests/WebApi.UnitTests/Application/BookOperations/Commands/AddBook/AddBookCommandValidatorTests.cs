using FluentAssertions;
using TestSetup;
using WebApi.Application.BookOperations.Commands.AddBook;

namespace Application.BookOperations.Commands.AddBook
{
    public class AddBookCommandValidatorTests : IClassFixture<CommonTestFixture>
    {
        [Theory]
        [InlineData("", 0, 0, 0)]
        [InlineData("", 0, 1, 0)]
        [InlineData("", 0, 0, 1)]
        [InlineData("", 0, 1, 1)]
        [InlineData("", 1, 0, 1)]
        [InlineData("", 1, 1, 0)]
        [InlineData("", 1, 1, 1)]
        [InlineData("RandomTitle", 0, 0, 0)]
        [InlineData("RandomTitle", 0, 1, 0)]
        [InlineData("RandomTitle", 0, 0, 1)]
        [InlineData("RandomTitle", 0, 1, 1)]
        [InlineData("RandomTitle", 1, 0, 1)]
        [InlineData("RandomTitle", 1, 1, 0)]
        public void WhenInvalidInputsAreGiven_Validator_ShouldBeReturnError(string title, int genreId, int pageCount, int authorId)
        {
            // arrange
            AddBookCommand command = new AddBookCommand(null, null);
            command.Model = new AddBookModel()
            {
                Title = title,
                GenreId = genreId,
                PageCount = pageCount,
                PublishDate = DateTime.Now.AddYears(-1),
                AuthorId = authorId
            };

            // act 
            AddBookCommandValidator validator = new AddBookCommandValidator();
            var result = validator.Validate(command);

            // assert
            result.Errors.Count.Should().BeGreaterThan(0);
        }

        [Fact]
        public void WhenDateTimeEqualNowGiven_Validator_ShouldBeReturnError()
        {
            // arrange
            AddBookCommand command = new AddBookCommand(null, null);
            command.Model = new AddBookModel()
            {
                Title = "Random",
                GenreId = 1,
                PageCount = 1,
                PublishDate = DateTime.Now.Date,
                AuthorId = 1
            };

            // act 
            AddBookCommandValidator validator = new AddBookCommandValidator();
            var result = validator.Validate(command);

            // assert
            result.Errors.Count.Should().BeGreaterThan(0);
        }

        [Fact]
        public void WhenValidInputAreGiven_Validator_ShouldNotBeReturnError()
        {
            // arrange
            AddBookCommand command = new AddBookCommand(null, null);
            command.Model = new AddBookModel()
            {
                Title = "Random",
                GenreId = 1,
                PageCount = 1,
                PublishDate = DateTime.Now.AddYears(-2),
                AuthorId = 1
            };

            // act 
            AddBookCommandValidator validator = new AddBookCommandValidator();
            var result = validator.Validate(command);

            // assert
            result.Errors.Count.Should().Be(0);
        }
    }
}
