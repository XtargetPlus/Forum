using Domain.UseCases.CreateForum;
using FluentAssertions;

namespace Domain.Tests.CreateForum;

public class CreateForumCommandValidatorShould
{
    private readonly CreateForumCommandValidator _sut = new();

    public static IEnumerable<object[]> GetInvalidCommand()
    {
        var validCommand = new CreateForumCommand("Test");
        
        yield return new object[] { validCommand with { Title = string.Empty } };
        yield return new object[] { validCommand with { Title = "Duo accusam ut. Kasd dolor justo vero sed dolore nonumy tation erat aliquyam labore clita gubergren. Gubergren et consequat amet nonumy diam consequat praesent sed lorem ipsum nostrud illum delenit. Dolores amet feugait ut diam." } };
    }

    [Fact]
    public void ReturnSuccess_WhenCommandValid()
    {
        var command = new CreateForumCommand("Test");
        _sut.Validate(command).IsValid.Should().BeTrue();
    }

    [Theory]
    [MemberData(nameof(GetInvalidCommand))]
    public void ReturnFailure_WhenCommandInvalid(CreateForumCommand command)
    {
        _sut.Validate(command).IsValid.Should().BeFalse();
    }
}