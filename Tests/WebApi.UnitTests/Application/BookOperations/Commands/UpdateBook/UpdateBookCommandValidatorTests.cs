using FluentAssertions;
using TestSetup;
using WebApi.Application.BookOperations.Commands.UpdateBook;

namespace Application.BookOperations.Commands.UpdateBook
{
    public class UpdateBookCommandValidatorTests : IClassFixture<CommonTestFixture>
    {
        [Theory]
        [InlineData(1, "", 0, 0, 0)]
        [InlineData(1, "", 0, 1, 0)]
        [InlineData(1, "", 0, 0, 1)]
        [InlineData(1, "", 0, 1, 1)]
        [InlineData(1, "", 1, 0, 1)]
        [InlineData(1, "", 1, 1, 0)]
        [InlineData(1, "", 1, 1, 1)]
        [InlineData(1, "RandomTitle", 0, 0, 0)]
        [InlineData(1, "RandomTitle", 0, 1, 0)]
        [InlineData(1, "RandomTitle", 0, 0, 1)]
        [InlineData(1, "RandomTitle", 0, 1, 1)]
        [InlineData(1, "RandomTitle", 1, 0, 1)]
        [InlineData(1, "RandomTitle", 1, 1, 0)]
        [InlineData(-1, "", 0, 0, 0)]
        [InlineData(-1, "", 0, 1, 0)]
        [InlineData(-1, "", 0, 0, 1)]
        [InlineData(-1, "", 0, 1, 1)]
        [InlineData(-1, "", 1, 0, 1)]
        [InlineData(-1, "", 1, 1, 0)]
        [InlineData(-1, "", 1, 1, 1)]
        [InlineData(-1, "RandomTitle", 0, 0, 0)]
        [InlineData(-1, "RandomTitle", 0, 1, 0)]
        [InlineData(-1, "RandomTitle", 0, 0, 1)]
        [InlineData(-1, "RandomTitle", 0, 1, 1)]
        [InlineData(-1, "RandomTitle", 1, 0, 1)]
        [InlineData(-1, "RandomTitle", 1, 1, 0)]
        public void WhenInvalidInputsAreGiven_Validator_ShouldBeReturnError(int bookId, string title, int genreId, int pageCount, int authorId)
        {
            // arrange
            UpdateBookCommand command = new UpdateBookCommand(null);
            command.BookId = bookId;
            command.Model = new UpdateBookModel()
            {
                Title = title,
                GenreId = genreId,
                PageCount = pageCount,
                PublishDate = DateTime.Now.AddYears(-1),
                AuthorId = authorId
            };

            // act 
            UpdateBookCommandValidator validator = new UpdateBookCommandValidator();
            var result = validator.Validate(command);

            // assert
            result.Errors.Count.Should().BeGreaterThan(0);
        }

        [Fact]
        public void WhenDateTimeEqualNowGiven_Validator_ShouldBeReturnError()
        {
            // arrange
            UpdateBookCommand command = new UpdateBookCommand(null);
            command.BookId = 1;
            command.Model = new UpdateBookModel()
            {
                Title = "Random",
                GenreId = 1,
                PageCount = 1,
                PublishDate = DateTime.Now.Date,
                AuthorId = 1
            };

            // act 
            UpdateBookCommandValidator validator = new UpdateBookCommandValidator();
            var result = validator.Validate(command);

            // assert
            result.Errors.Count.Should().BeGreaterThan(0);
        }

        [Fact]
        public void WhenValidInputAreGiven_Validator_ShouldNotBeReturnError()
        {
            // arrange
            UpdateBookCommand command = new UpdateBookCommand(null);
            command.BookId = 1;
            command.Model = new UpdateBookModel()
            {
                Title = "Random",
                GenreId = 1,
                PageCount = 1,
                PublishDate = DateTime.Now.AddYears(-2),
                AuthorId = 1
            };

            // act 
            UpdateBookCommandValidator validator = new UpdateBookCommandValidator();
            var result = validator.Validate(command);

            // assert
            result.Errors.Count.Should().Be(0);
        }
    }
}
