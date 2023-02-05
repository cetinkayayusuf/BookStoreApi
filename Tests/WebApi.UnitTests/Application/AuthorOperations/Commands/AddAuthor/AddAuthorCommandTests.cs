using AutoMapper;
using FluentAssertions;
using TestSetup;
using WebApi.Application.AuthorOperations.Commands.AddAuthor;
using WebApi.DBOperations;
using WebApi.Entities;

namespace Application.AuthorOperations.Commands.AddAuthor
{
    public class AddAuthorCommandTests : IClassFixture<CommonTestFixture>
    {
        private readonly BookStoreDbContext _context;
        private readonly IMapper _mapper;
        public AddAuthorCommandTests(CommonTestFixture testFixture)
        {
            _context = testFixture.Context;
            _mapper = testFixture.Mapper;
        }

        [Fact]
        public void WhenAlreadyExistAuthorNameIsGiven_InvalidOperationException_ShouldBeReturn()
        {
            //arrange
            var author = new Author()
            {
                FirstName = "WhenAlreadyExistAuthorNameIsGiven_InvalidOperationException_ShouldBeReturn",
                LastName = "WhenAlreadyExistAuthorNameIsGiven_InvalidOperationException_ShouldBeReturn",
                BirthDate = new DateTime(2004, 4, 24)
            };
            _context.Authors.Add(author);
            _context.SaveChanges();

            AddAuthorCommand command = new AddAuthorCommand(_context, _mapper);
            command.Model = new AddAuthorModel() { FirstName = author.FirstName, LastName = author.LastName, BirthDate = author.BirthDate };

            //act and assert
            FluentActions
            .Invoking(() => command.Handle())
            .Should().Throw<InvalidOperationException>().And.Message.Should().Be("Author exist");
        }

        [Fact]
        public void WhenValidInputsAreGiven_Author_ShouldBeAdded()
        {
            // arrange
            AddAuthorCommand command = new AddAuthorCommand(_context, _mapper);
            AddAuthorModel model = new AddAuthorModel()
            {
                FirstName = "WhenValidInputsAreGiven_Author_ShouldBeAdded",
                LastName = "WhenValidInputsAreGiven_Author_ShouldBeAdded",
                BirthDate = new DateTime(2004, 4, 24)
            };
            command.Model = model;

            // act
            FluentActions
            .Invoking(() => command.Handle()).Invoke();

            //act and assert
            var author = _context.Authors.SingleOrDefault(x => x.FirstName == model.FirstName && x.LastName == model.LastName);
            author.Should().NotBeNull();
            author.FirstName.Should().Be(model.FirstName);
            author.LastName.Should().Be(model.LastName);
            author.BirthDate.Should().Be(model.BirthDate);
        }
    }
}
