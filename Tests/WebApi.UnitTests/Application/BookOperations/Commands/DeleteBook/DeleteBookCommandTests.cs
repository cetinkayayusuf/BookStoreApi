using FluentAssertions;
using TestSetup;
using WebApi.Application.BookOperations.Commands.DeleteBook;
using WebApi.DBOperations;
using WebApi.Entities;

namespace Application.BookOperations.Commands.DeleteBook
{
    public class DeleteBookCommandTests : IClassFixture<CommonTestFixture>
    {
        private readonly BookStoreDbContext _context;
        public DeleteBookCommandTests(CommonTestFixture testFixture)
        {
            _context = testFixture.Context;
        }

        [Fact]
        public void WhenNotExistBookIdIsGiven_InvalidOperationException_ShouldBeReturn()
        {
            //arrange
            DeleteBookCommand command = new DeleteBookCommand(_context);
            command.BookId = -1;

            //act and assert
            FluentActions
            .Invoking(() => command.Handle())
            .Should().Throw<InvalidOperationException>().And.Message.Should().Be("Book not found");
        }

        [Fact]
        public void WhenExistBookIdIsGiven_Book_ShouldBeDeleted()
        {
            //arrange
            var book = new Book()
            {
                Title = "WhenExistBookIdIsGiven_Book_ShouldBeDeleted",
                GenreId = 1,
                PageCount = 1,
                PublishDate = new DateTime(2004, 4, 24),
                AuthorId = 1
            };

            _context.Books.Add(book);
            _context.SaveChanges();

            var bookId = _context.Books.SingleOrDefault(x => x.Title == book.Title).Id;
            DeleteBookCommand command = new DeleteBookCommand(_context);
            command.BookId = bookId;

            //act
            FluentActions
                .Invoking(() => command.Handle()).Invoke();

            //assert
            book = _context.Books.SingleOrDefault(x => x.Id == bookId);
            book.Should().BeNull();
        }
    }
}
