using AutoMapper;
using FluentAssertions;
using TestSetup;
using WebApi.Application.AuthorOperations.Queries.GetAuthorDetail;
using WebApi.DBOperations;
using WebApi.Entities;

namespace Application.AuthorOperations.Queries.GetAuthorDetail
{
    public class GetAuthorQueryTests : IClassFixture<CommonTestFixture>
    {
        private readonly BookStoreDbContext _context;
        private readonly IMapper _mapper;
        public GetAuthorQueryTests(CommonTestFixture testFixture)
        {
            _context = testFixture.Context;
            _mapper = testFixture.Mapper;
        }

        [Fact]
        public void WhenNotExistAuthorIdIsGiven_InvalidOperationException_ShouldBeReturn()
        {
            // arrange
            GetAuthorDetailQuery query = new GetAuthorDetailQuery(_context, _mapper);
            query.AuthorId = -1;
            // act and assert
            FluentActions
                .Invoking(() => query.Handle())
                .Should().Throw<InvalidOperationException>().And.Message.Should().Be("Author not found");
        }

        [Fact]
        public void WhenExistAuthorIdIsGiven_AuthorDetailViewModel_ShouldBeReturn()
        {
            //arrange
            var author = new Author()
            {
                FirstName = "WhenExistAuthorIdIsGiven_AuthorDetailViewModel_ShouldBeReturn",
                LastName = "WhenExistAuthorIdIsGiven_AuthorDetailViewModel_ShouldBeReturn",
                BirthDate = DateTime.Now.AddYears(-20),
            };
            if (!_context.Authors.Any(x => x.FirstName == author.FirstName && x.LastName == author.LastName))
            {
                _context.Authors.Add(author);
                _context.SaveChanges();
            }

            var authorId = _context.Authors.SingleOrDefault(x => x.FirstName == author.FirstName && x.LastName == author.LastName).Id;

            // arrange
            GetAuthorDetailQuery query = new GetAuthorDetailQuery(_context, _mapper);
            query.AuthorId = authorId;

            // act
            var result = FluentActions.Invoking(() => query.Handle()).Invoke();

            // assert
            result.FirstName.Should().NotBeNull();
        }
    }
}
