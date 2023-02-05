using FluentAssertions;
using TestSetup;
using WebApi.Application.GenreOperations.Commands.UpdateGenre;
using WebApi.DBOperations;
using WebApi.Entities;

namespace Application.GenreOperations.Commands.UpdateGenre
{
    public class UpdateGenreCommandTests : IClassFixture<CommonTestFixture>
    {
        private readonly BookStoreDbContext _context;
        public UpdateGenreCommandTests(CommonTestFixture fixture)
        {
            _context = fixture.Context;
        }

        [Fact]
        public void WhenNotExistGenreIdIsGiven_InvalidOperationException_ShouldBeReturn()
        {
            // arrange
            UpdateGenreCommand command = new UpdateGenreCommand(_context);
            command.GenreId = -1;
            // act and assert
            FluentActions
                .Invoking(() => command.Handle())
                .Should().Throw<InvalidOperationException>().And.Message.Should().Be("Genre not found");
        }

        [Fact]
        public void WhenExistGenreIdAndUpdateGenreModelAreGiven_Genre_ShouldBeUpdated()
        {
            // arrange
            var genre = new Genre()
            {
                Name = "WhenExistGenreIdAndValidUpdateInputAreGiven_Genre_ShouldBeUpdated"
            };
            _context.Genres.Add(genre);
            _context.SaveChanges();

            var genreId = _context.Genres.SingleOrDefault(x => x.Name == genre.Name).Id;

            var updateModel = new UpdateGenreModel()
            {
                Name = genre.Name + "Updated"
            };

            UpdateGenreCommand command = new UpdateGenreCommand(_context);
            command.GenreId = genreId;
            command.Model = updateModel;


            // act
            FluentActions
                .Invoking(() => command.Handle()).Invoke();

            // assert
            _context.Genres.Any(
                x => x.Id == genreId && x.Name == updateModel.Name
                ).Should().BeTrue();
        }

        [Fact]
        public void WhenUpdateGenreModelContainsEmptyValue_Genre_ShouldBeUpdatedUsingExistingValue()
        {
            // arrange
            var genre = new Genre()
            {
                Name = "WhenUpdateGenreModelContainsDefaultValue_Genre_ShouldBeUpdatedUsingExistingValue"
            };
            if (!_context.Genres.Any(x => x.Name == genre.Name))
            {
                _context.Genres.Add(genre);
                _context.SaveChanges();
            }

            var genreId = _context.Genres.SingleOrDefault(x => x.Name == genre.Name).Id;

            var updateModel = new UpdateGenreModel()
            {
                Name = ""
            };

            UpdateGenreCommand command = new UpdateGenreCommand(_context);
            command.GenreId = genreId;
            command.Model = updateModel;


            // act
            FluentActions
                .Invoking(() => command.Handle()).Invoke();

            // assert
            _context.Genres.Any(
                x => x.Id == genreId && x.Name == genre.Name
            ).Should().BeTrue();
        }
    }
}
