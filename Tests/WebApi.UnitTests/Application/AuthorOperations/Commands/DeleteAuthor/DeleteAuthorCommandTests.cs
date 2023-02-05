using FluentAssertions;
using TestSetup;
using WebApi.Application.AuthorOperations.Commands.DeleteAuthor;
using WebApi.DBOperations;
using WebApi.Entities;

namespace Application.AuthorOperations.Commands.DeleteAuthor
{
    public class DeleteAuthorCommandTests : IClassFixture<CommonTestFixture>
    {
        private readonly BookStoreDbContext _context;
        public DeleteAuthorCommandTests(CommonTestFixture testFixture)
        {
            _context = testFixture.Context;
        }

        [Fact]
        public void WhenNotExistAuthorIdIsGiven_InvalidOperationException_ShouldBeReturn()
        {
            //arrange
            DeleteAuthorCommand command = new DeleteAuthorCommand(_context);
            command.AuthorId = -1;

            //act and assert
            FluentActions
            .Invoking(() => command.Handle())
            .Should().Throw<InvalidOperationException>().And.Message.Should().Be("Author not found");
        }

        [Fact]
        public void WhenExistAuthorIdIsGivenAndAuthorHasNoBooks_Author_ShouldBeDeleted()
        {
            //arrange
            var author = new Author()
            {
                FirstName = "WhenExistAuthorIdIsGiven_Author_ShouldBeDeleted",
                LastName = "WhenExistAuthorIdIsGiven_Author_ShouldBeDeleted",
                BirthDate = new DateTime(2004, 4, 24),
            };

            _context.Authors.Add(author);
            _context.SaveChanges();

            var authorId = _context.Authors.SingleOrDefault(x => x.FirstName == author.FirstName && x.LastName == author.LastName).Id;
            DeleteAuthorCommand command = new DeleteAuthorCommand(_context);
            command.AuthorId = authorId;

            //act
            FluentActions
                .Invoking(() => command.Handle()).Invoke();

            //assert
            author = _context.Authors.SingleOrDefault(x => x.Id == authorId);
            author.Should().BeNull();
        }

        [Fact]
        public void WhenExistAuthorIdIsGivenAndAuthorHasBooks_InvalidOperationException_ShouldBeReturn()
        {
            //arrange
            var author = new Author()
            {
                FirstName = "WhenExistAuthorIdIsGiven_Author_ShouldBeDeleted",
                LastName = "WhenExistAuthorIdIsGiven_Author_ShouldBeDeleted",
                BirthDate = new DateTime(2004, 4, 24)
            };

            _context.Authors.Add(author);
            _context.SaveChanges();

            var authorId = _context.Authors.SingleOrDefault(x => x.FirstName == author.FirstName && x.LastName == author.LastName).Id;

            var book = new Book()
            {
                Title = "WhenExistAuthorIdIsGivenAndAuthorHasBooks_InvalidOperationException_ShouldBeReturn",
                GenreId = 1,
                PageCount = 1,
                PublishDate = DateTime.Now.AddYears(-5),
                AuthorId = authorId
            };
            _context.Books.Add(book);
            _context.SaveChanges();

            DeleteAuthorCommand command = new DeleteAuthorCommand(_context);
            command.AuthorId = authorId;

            //act and assert
            FluentActions
            .Invoking(() => command.Handle())
            .Should().Throw<InvalidOperationException>().And.Message.Should().Be("Author has book(s), cannot delete");
        }
    }
}
