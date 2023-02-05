using FluentAssertions;
using TestSetup;
using WebApi.Application.AuthorOperations.Queries.GetAuthorDetail;

namespace Application.AuthorOperations.Queries.GetAuthorDetail
{
    public class GetAuthorQueryValidatorTests : IClassFixture<CommonTestFixture>
    {
        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        public void WhenInvalidInputsAreGiven_Validator_ShouldBeReturnError(int authorId)
        {
            // arrange
            GetAuthorDetailQuery query = new GetAuthorDetailQuery(null, null);
            query.AuthorId = authorId;

            // act 
            GetAuthorDetailQueryValidator validator = new GetAuthorDetailQueryValidator();
            var result = validator.Validate(query);

            // assert
            result.Errors.Count.Should().BeGreaterThan(0);
        }

        [Fact]
        public void WhenValidInputAreGiven_Validator_ShouldNotBeReturnError()
        {
            // arrange
            GetAuthorDetailQuery query = new GetAuthorDetailQuery(null, null);
            query.AuthorId = 1;

            // act 
            GetAuthorDetailQueryValidator validator = new GetAuthorDetailQueryValidator();
            var result = validator.Validate(query);

            // assert
            result.Errors.Count.Should().Be(0);
        }
    }
}
