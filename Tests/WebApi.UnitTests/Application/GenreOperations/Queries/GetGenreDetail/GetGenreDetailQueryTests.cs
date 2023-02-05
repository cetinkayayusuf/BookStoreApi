using AutoMapper;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using TestSetup;
using WebApi.Application.GenreOperations.Queries.GetGenreDetail;
using WebApi.DBOperations;
using WebApi.Entities;

namespace Application.GenreOperations.Queries.GetGenreDetail
{
    public class GetGenreQueryTests : IClassFixture<CommonTestFixture>
    {
        private readonly BookStoreDbContext _context;
        private readonly IMapper _mapper;
        public GetGenreQueryTests(CommonTestFixture testFixture)
        {
            _context = testFixture.Context;
            _mapper = testFixture.Mapper;
        }

        [Fact]
        public void WhenNotExistGenreIdIsGiven_InvalidOperationException_ShouldBeReturn()
        {
            // arrange
            GetGenreDetailQuery query = new GetGenreDetailQuery(_context, _mapper);
            query.GenreId = -1;
            // act and assert
            FluentActions
                .Invoking(() => query.Handle())
                .Should().Throw<InvalidOperationException>().And.Message.Should().Be("Genre not found");
        }

        [Fact]
        public void WhenExistGenreIdIsGivenAndGenreIsActive_GenreDetailViewModel_ShouldBeReturn()
        {
            //arrange
            var genre = new Genre()
            {
                Name = "WhenExistGenreIdIsGiven_GenreDetailViewModel_ShouldBeReturn"
            };
            if (!_context.Genres.Any(x => x.Name == genre.Name))
            {
                _context.Genres.Add(genre);
                _context.SaveChanges();
            }

            var addedGenre = _context.Genres.SingleOrDefault(x => x.Name == genre.Name);
            addedGenre.IsActive = true;
            _context.SaveChanges();

            var genreId = addedGenre.Id;

            // arrange
            GetGenreDetailQuery query = new GetGenreDetailQuery(_context, _mapper);
            query.GenreId = genreId;

            // act
            var result = FluentActions.Invoking(() => query.Handle()).Invoke();

            // assert
            result.Name.Should().NotBeNull();
        }

        [Fact]
        public void WhenExistGenreIdIsGivenAndGenreIsNotActive_InvalidOperationException_ShouldBeReturn()
        {
            //arrange
            var genre = new Genre()
            {
                Name = "WhenExistGenreIdIsGiven_GenreDetailViewModel_ShouldBeReturn"
            };
            _context.Genres.Add(genre);
            _context.SaveChanges();

            var addedGenre = _context.Genres.SingleOrDefault(x => x.Name == genre.Name);
            addedGenre.IsActive = false;
            _context.SaveChanges();

            var genreId = addedGenre.Id;

            // arrange
            GetGenreDetailQuery query = new GetGenreDetailQuery(_context, _mapper);
            query.GenreId = genreId;

            // act and assert
            FluentActions.Invoking(() => query.Handle())
                .Should().Throw<InvalidOperationException>().And.Message.Should().Be("Genre not found");
        }
    }
}
