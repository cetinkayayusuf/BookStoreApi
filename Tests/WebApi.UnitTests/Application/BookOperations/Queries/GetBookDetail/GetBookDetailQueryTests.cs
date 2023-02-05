using AutoMapper;
using FluentAssertions;
using TestSetup;
using WebApi.Application.BookOperations.Queries.GetBookDetail;
using WebApi.DBOperations;
using WebApi.Entities;

namespace Application.BookOperations.Queries.GetBookDetail
{
    public class GetBookQueryTests : IClassFixture<CommonTestFixture>
    {
        private readonly BookStoreDbContext _context;
        private readonly IMapper _mapper;
        public GetBookQueryTests(CommonTestFixture testFixture)
        {
            _context = testFixture.Context;
            _mapper = testFixture.Mapper;
        }

        [Fact]
        public void WhenNotExistBookIdIsGiven_InvalidOperationException_ShouldBeReturn()
        {
            // arrange
            GetBookDetailQuery query = new GetBookDetailQuery(_context, _mapper);
            query.BookId = -1;
            // act and assert
            FluentActions
                .Invoking(() => query.Handle())
                .Should().Throw<InvalidOperationException>().And.Message.Should().Be("Book not found");
        }

        [Fact]
        public void WhenExistBookIdIsGiven_BookDetailViewModel_ShouldBeReturn()
        {
            //arrange
            var book = new Book()
            {
                Title = "WhenExistBookIdIsGiven_BookDetailViewModel_ShouldBeReturn",
                GenreId = 2,
                PageCount = 2,
                PublishDate = new DateTime(2004, 4, 24),
                AuthorId = 2
            };
            if (!_context.Books.Any(x => x.Title == book.Title))
            {
                _context.Books.Add(book);
                _context.SaveChanges();
            }

            var bookId = _context.Books.SingleOrDefault(x => x.Title == book.Title).Id;

            // arrange
            GetBookDetailQuery query = new GetBookDetailQuery(_context, _mapper);
            query.BookId = bookId;

            // act
            var result = FluentActions.Invoking(() => query.Handle()).Invoke();

            // assert
            result.Should().NotBeNull();
        }
    }
}