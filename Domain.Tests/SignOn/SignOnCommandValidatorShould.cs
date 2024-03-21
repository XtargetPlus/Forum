using Domain.UseCases.SignOn;
using FluentAssertions;

namespace Domain.Tests.SignOn;

public class SignOnCommandValidatorShould
{
    private readonly SignOnCommandValidator _sut = new();

    public static IEnumerable<object[]> GetInvalidCommand()
    {
        var validCommand = new SignOnCommand("Qwerty1", "Qwerty2");

        yield return new object[] { validCommand with { Login = string.Empty,  Password = string.Empty} };
        yield return new object[] { validCommand with { Password = string.Empty } };
        yield return new object[] { validCommand with { Login = string.Empty } };
        yield return new object[] { validCommand with { Login = "Duo accusam ut. Kasd dolor justo vero sed dolore nonumy tation erat aliquyam labore clita gubergren. Gubergren et consequat amet nonumy diam consequat praesent sed lorem ipsum nostrud illum delenit. Dolores amet feugait ut diam.", } };
    }

    [Fact]
    public void ReturnSuccess_WhenCommandValid()
    {
        var command = new SignOnCommand("Qwerty1", "Qwerty2");
        _sut.Validate(command).IsValid.Should().BeTrue();
    }

    [Theory]
    [MemberData(nameof(GetInvalidCommand))]
    public void ReturnFailure_WhenCommandInvalid(SignOnCommand command)
    {
        _sut.Validate(command).IsValid.Should().BeFalse();
    }
}