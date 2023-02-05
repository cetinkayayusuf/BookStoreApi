using AutoMapper;
using FluentAssertions;
using TestSetup;
using WebApi.Application.BookOperations.Commands.AddBook;
using WebApi.DBOperations;
using WebApi.Entities;

namespace Application.BookOperations.Commands.AddBook
{
    public class AddBookCommandTests : IClassFixture<CommonTestFixture>
    {
        private readonly BookStoreDbContext _context;
        private readonly IMapper _mapper;
        public AddBookCommandTests(CommonTestFixture testFixture)
        {
            _context = testFixture.Context;
            _mapper = testFixture.Mapper;
        }

        [Fact]
        public void WhenAlreadyExistBookTitleIsGiven_InvalidOperationException_ShouldBeReturn()
        {
            //arrange
            var book = new Book()
            {
                Title = "Test_WhenAlreadyExistBookTitleIsGiven_InvalidOperationException_ShouldBeReturn",
                GenreId = 1,
                PageCount = 1,
                PublishDate = new DateTime(2004, 4, 24),
                AuthorId = 1
            };
            _context.Books.Add(book);
            _context.SaveChanges();

            AddBookCommand command = new AddBookCommand(_context, _mapper);
            command.Model = new AddBookModel() { Title = book.Title };

            //act and assert
            FluentActions
            .Invoking(() => command.Handle())
            .Should().Throw<InvalidOperationException>().And.Message.Should().Be("Book exist");
        }

        [Fact]
        public void WhenValidInputsAreGiven_Book_ShouldBeAdded()
        {
            // arrange
            AddBookCommand command = new AddBookCommand(_context, _mapper);
            AddBookModel model = new AddBookModel()
            {
                Title = "WhenValidInputsAreGiven_Book_ShouldBeAdded",
                GenreId = 1,
                PageCount = 1,
                PublishDate = new DateTime(2004, 4, 24),
                AuthorId = 1
            };
            command.Model = model;

            // act
            FluentActions
            .Invoking(() => command.Handle()).Invoke();

            //act and assert
            var book = _context.Books.SingleOrDefault(x => x.Title == model.Title);
            book.Should().NotBeNull();
            book.GenreId.Should().Be(model.GenreId);
            book.PageCount.Should().Be(model.PageCount);
            book.PublishDate.Should().Be(model.PublishDate);
            book.AuthorId.Should().Be(model.AuthorId);
        }
    }
}
