using FluentAssertions;
using TestSetup;
using WebApi.Application.GenreOperations.Queries.GetGenreDetail;

namespace Application.GenreOperations.Queries.GetGenreDetail
{
    public class GetGenreQueryValidatorTests : IClassFixture<CommonTestFixture>
    {
        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        public void WhenInvalidInputsAreGiven_Validator_ShouldBeReturnError(int genreId)
        {
            // arrange
            GetGenreDetailQuery query = new GetGenreDetailQuery(null, null);
            query.GenreId = genreId;

            // act 
            GetGenreDetailQueryValidator validator = new GetGenreDetailQueryValidator();
            var result = validator.Validate(query);

            // assert
            result.Errors.Count.Should().BeGreaterThan(0);
        }

        [Fact]
        public void WhenValidInputAreGiven_Validator_ShouldNotBeReturnError()
        {
            // arrange
            GetGenreDetailQuery query = new GetGenreDetailQuery(null, null);
            query.GenreId = 1;

            // act 
            GetGenreDetailQueryValidator validator = new GetGenreDetailQueryValidator();
            var result = validator.Validate(query);

            // assert
            result.Errors.Count.Should().Be(0);
        }
    }
}
