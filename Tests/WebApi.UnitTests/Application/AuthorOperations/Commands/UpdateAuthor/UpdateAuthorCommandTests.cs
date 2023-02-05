using FluentAssertions;
using TestSetup;
using WebApi.Application.AuthorOperations.Commands.UpdateAuthor;
using WebApi.DBOperations;
using WebApi.Entities;

namespace Application.AuthorOperations.Commands.UpdateAuthor
{
    public class UpdateAuthorCommandTests : IClassFixture<CommonTestFixture>
    {
        private readonly BookStoreDbContext _context;
        public UpdateAuthorCommandTests(CommonTestFixture fixture)
        {
            _context = fixture.Context;
        }

        [Fact]
        public void WhenNotExistAuthorIdIsGiven_InvalidOperationException_ShouldBeReturn()
        {
            // arrange
            UpdateAuthorCommand command = new UpdateAuthorCommand(_context);
            command.AuthorId = -1;
            // act and assert
            FluentActions
                .Invoking(() => command.Handle())
                .Should().Throw<InvalidOperationException>().And.Message.Should().Be("Author not found");
        }

        [Fact]
        public void WhenExistAuthorIdAndUpdateAuthorModelAreGiven_Author_ShouldBeUpdated()
        {
            // arrange
            var author = new Author()
            {
                FirstName = "WhenExistAuthorIdAndUpdateAuthorModelAreGiven_Author_ShouldBeUpdated",
                LastName = "WhenExistAuthorIdAndUpdateAuthorModelAreGiven_Author_ShouldBeUpdated",
                BirthDate = DateTime.Now.AddYears(-2),
            };
            _context.Authors.Add(author);
            _context.SaveChanges();

            var authorId = _context.Authors.SingleOrDefault(x => x.FirstName == author.FirstName && x.LastName == author.LastName).Id;

            var updateModel = new UpdateAuthorModel()
            {
                FirstName = author.FirstName + "Updated",
                LastName = author.LastName + "Updated",
                BirthDate = author.BirthDate.AddYears(-2),
            };

            UpdateAuthorCommand command = new UpdateAuthorCommand(_context);
            command.AuthorId = authorId;
            command.Model = updateModel;


            // act
            FluentActions
                .Invoking(() => command.Handle()).Invoke();

            // assert
            _context.Authors.Any(x => x.Id == authorId
                            && x.FirstName == updateModel.FirstName
                            && x.LastName == updateModel.LastName
                            && x.BirthDate == updateModel.BirthDate
            ).Should().BeTrue();
        }

        [Theory]
        [InlineData(default, "aaaaaa", false)]
        [InlineData(default, default, false)]
        [InlineData("aaaaa", default, false)]
        [InlineData(default, "aaaaaa", true)]
        [InlineData("aaaaa", "aaaaaa", true)]
        [InlineData(default, default, true)]
        [InlineData("aaaaa", default, true)]
        public void WhenUpdateAuthorModelContainsDefaultValue_Author_ShouldBeUpdatedUsingExistingValue(string firstName, string lastName, bool birthDateIsDefault)
        {
            // arrange
            var author = new Author()
            {
                FirstName = "WhenUpdateAuthorModelContainsDefaultValue_Author_ShouldBeUpdatedUsingExistingValue",
                LastName = "WhenUpdateAuthorModelContainsDefaultValue_Author_ShouldBeUpdatedUsingExistingValue",
                BirthDate = DateTime.Now.Date.AddYears(-20),
            };
            if (!_context.Authors.Any(x => x.FirstName == author.FirstName && x.LastName == author.LastName))
            {
                _context.Authors.Add(author);
                _context.SaveChanges();
            }

            var authorId = _context.Authors.SingleOrDefault(x => x.FirstName == author.FirstName && x.LastName == author.LastName).Id;
            Console.WriteLine(authorId);
            var updateModel = new UpdateAuthorModel()
            {
                FirstName = firstName,
                LastName = lastName,
                BirthDate = birthDateIsDefault ? default : author.BirthDate.AddYears(-5).Date,
            };

            UpdateAuthorCommand command = new UpdateAuthorCommand(_context);
            command.AuthorId = authorId;
            command.Model = updateModel;


            // act
            FluentActions
                .Invoking(() => command.Handle()).Invoke();

            // assert
            var result = _context.Authors.SingleOrDefault(x => x.Id == authorId
                            && x.FirstName == (firstName == default ? author.FirstName : updateModel.FirstName)
                            && x.LastName == (lastName == default ? author.LastName : updateModel.LastName)
                            && x.BirthDate == (birthDateIsDefault ? author.BirthDate : updateModel.BirthDate)
            );
            result.Should().NotBeNull();
        }
    }
}
