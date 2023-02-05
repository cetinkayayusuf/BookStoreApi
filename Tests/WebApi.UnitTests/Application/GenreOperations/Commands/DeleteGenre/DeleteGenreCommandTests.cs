using FluentAssertions;
using TestSetup;
using WebApi.Application.GenreOperations.Commands.DeleteGenre;
using WebApi.DBOperations;
using WebApi.Entities;

namespace Application.GenreOperations.Commands.DeleteGenre
{
    public class DeleteGenreCommandTests : IClassFixture<CommonTestFixture>
    {
        private readonly BookStoreDbContext _context;
        public DeleteGenreCommandTests(CommonTestFixture testFixture)
        {
            _context = testFixture.Context;
        }

        [Fact]
        public void WhenNotExistGenreIdIsGiven_InvalidOperationException_ShouldBeReturn()
        {
            //arrange
            DeleteGenreCommand command = new DeleteGenreCommand(_context);
            command.GenreId = -1;

            //act and assert
            FluentActions
            .Invoking(() => command.Handle())
            .Should().Throw<InvalidOperationException>().And.Message.Should().Be("Genre not found");
        }

        [Fact]
        public void WhenExistGenreIdIsGiven_Genre_ShouldBeDeleted()
        {
            //arrange
            var genre = new Genre()
            {
                Name = "WhenExistGenreIdIsGiven_Genre_ShouldBeDeleted"
            };

            _context.Genres.Add(genre);
            _context.SaveChanges();

            var genreId = _context.Genres.SingleOrDefault(x => x.Name == genre.Name).Id;
            DeleteGenreCommand command = new DeleteGenreCommand(_context);
            command.GenreId = genreId;

            //act
            FluentActions
                .Invoking(() => command.Handle()).Invoke();

            //assert
            genre = _context.Genres.SingleOrDefault(x => x.Id == genreId);
            genre.Should().BeNull();
        }
    }
}
