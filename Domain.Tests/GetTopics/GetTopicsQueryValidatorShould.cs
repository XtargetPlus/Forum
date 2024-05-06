using FluentAssertions;
using Forum.Domain.UseCases.GetTopics;

namespace Forum.Domain.Tests.GetTopics;

public class GetTopicsQueryValidatorShould
{
    private readonly GetTopicsQueryValidator _sut = new();

    [Fact]
    public void ReturnSuccess_WhenQueryIsValid()
    {
        var query = new GetTopicsQuery(Guid.Parse("f8d2a603-59ea-4578-81ee-9264fa851765"), 10, 5);
        _sut.Validate(query).IsValid.Should().BeTrue();
    }

    public static IEnumerable<object[]> GetInvalidQuery()
    {
        var query = new GetTopicsQuery(Guid.Parse("f8d2a603-59ea-4578-81ee-9264fa851765"), 10, 5);
        yield return new object[] { query with { ForumId = Guid.Empty } };
        yield return new object[] { query with { Skip = -10 } };
        yield return new object[] { query with { Take = -10 } };
    }

    [Theory]
    [MemberData(nameof(GetInvalidQuery))]
    public void ReturnFailure_WhenQueryIsInvalid(GetTopicsQuery query)
    {
        _sut.Validate(query).IsValid.Should().BeFalse();
    }
}