using AutoMapper;
using FluentAssertions;
using TestSetup;
using WebApi.Application.GenreOperations.Commands.AddGenre;
using WebApi.DBOperations;
using WebApi.Entities;

namespace Application.GenreOperations.Commands.AddGenre
{
    public class AddGenreCommandTests : IClassFixture<CommonTestFixture>
    {
        private readonly BookStoreDbContext _context;
        private readonly IMapper _mapper;
        public AddGenreCommandTests(CommonTestFixture testFixture)
        {
            _context = testFixture.Context;
            _mapper = testFixture.Mapper;
        }

        [Fact]
        public void WhenAlreadyExistGenreNameIsGiven_InvalidOperationException_ShouldBeReturn()
        {
            //arrange
            var genre = new Genre()
            {
                Name = "WhenAlreadyExistGenreNameIsGiven_InvalidOperationException_ShouldBeReturn"
            };
            _context.Genres.Add(genre);
            _context.SaveChanges();

            AddGenreCommand command = new AddGenreCommand(_context, _mapper);
            command.Model = new AddGenreModel() { Name = genre.Name };

            //act and assert
            FluentActions
            .Invoking(() => command.Handle())
            .Should().Throw<InvalidOperationException>().And.Message.Should().Be("Genre exist");
        }

        [Fact]
        public void WhenValidInputsAreGiven_Genre_ShouldBeAdded()
        {
            // arrange
            AddGenreCommand command = new AddGenreCommand(_context, _mapper);
            AddGenreModel model = new AddGenreModel()
            {
                Name = "WhenValidInputsAreGiven_Genre_ShouldBeAdded"
            };
            command.Model = model;

            // act
            FluentActions
            .Invoking(() => command.Handle()).Invoke();

            //act and assert
            var genre = _context.Genres.SingleOrDefault(x => x.Name == model.Name);
            genre.Should().NotBeNull();
        }
    }
}
