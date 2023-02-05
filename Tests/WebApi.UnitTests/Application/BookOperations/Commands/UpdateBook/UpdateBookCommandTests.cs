using FluentAssertions;
using TestSetup;
using WebApi.Application.BookOperations.Commands.UpdateBook;
using WebApi.DBOperations;
using WebApi.Entities;

namespace Application.BookOperations.Commands.UpdateBook
{
    public class UpdateBookCommandTests : IClassFixture<CommonTestFixture>
    {
        private readonly BookStoreDbContext _context;
        public UpdateBookCommandTests(CommonTestFixture fixture)
        {
            _context = fixture.Context;
        }

        [Fact]
        public void WhenNotExistBookIdIsGiven_InvalidOperationException_ShouldBeReturn()
        {
            // arrange
            UpdateBookCommand command = new UpdateBookCommand(_context);
            command.BookId = -1;
            // act and assert
            FluentActions
                .Invoking(() => command.Handle())
                .Should().Throw<InvalidOperationException>().And.Message.Should().Be("Book not found");
        }

        [Fact]
        public void WhenExistBookIdAndUpdateBookModelAreGiven_Book_ShouldBeUpdated()
        {
            // arrange
            var book = new Book()
            {
                Title = "WhenExistBookIdAndValidUpdateInputAreGiven_Book_ShouldBeUpdated",
                GenreId = 1,
                PageCount = 1,
                PublishDate = new DateTime(2004, 4, 24),
                AuthorId = 1,
            };
            _context.Books.Add(book);
            _context.SaveChanges();

            var bookId = _context.Books.SingleOrDefault(x => x.Title == book.Title).Id;

            var updateModel = new UpdateBookModel()
            {
                Title = book.Title + "Updated",
                GenreId = book.GenreId + 1,
                PageCount = book.PageCount + 1,
                PublishDate = book.PublishDate.AddYears(-5).Date,
                AuthorId = book.AuthorId + 1,
            };

            UpdateBookCommand command = new UpdateBookCommand(_context);
            command.BookId = bookId;
            command.Model = updateModel;

            // act
            FluentActions
                .Invoking(() => command.Handle()).Invoke();

            // assert
            _context.Books.Any(x => x.Id == bookId
                            && x.Title == updateModel.Title
                            && x.GenreId == updateModel.GenreId
                            && x.PageCount == updateModel.PageCount
                            && x.PublishDate == updateModel.PublishDate
                            && x.AuthorId == updateModel.AuthorId
            ).Should().BeTrue();
        }

        [Theory]
        [InlineData(default, 1, 1, false, 1)]
        [InlineData("aaaa", default, 1, false, 1)]
        [InlineData("aaaa", 1, default, false, 1)]
        [InlineData("aaaa", 1, 1, true, 1)]
        [InlineData("aaaa", 1, 1, true, default)]
        public void WhenUpdateBookModelContainsDefaultValue_Book_ShouldBeUpdatedUsingExistingValue(string title, int genreId, int pageCount, bool publishDateIsDefault, int authorId)
        {
            // arrange
            var book = new Book()
            {
                Title = "WhenUpdateBookModelContainsDefaultValue_Book_ShouldBeUpdatedUsingExistingValue",
                GenreId = 1,
                PageCount = 1,
                PublishDate = new DateTime(2004, 4, 24),
                AuthorId = 1,
            };
            if (!_context.Books.Any(x => x.Title == book.Title))
            {
                _context.Books.Add(book);
                _context.SaveChanges();
            }

            var bookId = _context.Books.SingleOrDefault(x => x.Title == book.Title).Id;

            var updateModel = new UpdateBookModel()
            {
                Title = title,
                GenreId = genreId,
                PageCount = pageCount,
                PublishDate = publishDateIsDefault ? default : book.PublishDate.AddYears(-5).Date,
                AuthorId = authorId,
            };

            UpdateBookCommand command = new UpdateBookCommand(_context);
            command.BookId = bookId;
            command.Model = updateModel;

            // act
            FluentActions
                .Invoking(() => command.Handle()).Invoke();

            // assert
            _context.Books.Any(x => x.Id == bookId
                            && x.Title == (title == default ? book.Title : updateModel.Title)
                            && x.GenreId == (genreId == default ? book.GenreId : updateModel.GenreId)
                            && x.PageCount == (pageCount == default ? book.PageCount : updateModel.PageCount)
                            && x.PublishDate == (publishDateIsDefault ? book.PublishDate : updateModel.PublishDate)
                            && x.AuthorId == (authorId == default ? book.AuthorId : updateModel.AuthorId)
            ).Should().BeTrue();
        }
    }
}
