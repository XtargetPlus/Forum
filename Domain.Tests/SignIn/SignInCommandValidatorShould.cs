using FluentAssertions;
using Forum.Domain.UseCases.SignIn;

namespace Forum.Domain.Tests.SignIn;

public class SignInCommandValidatorShould
{
    private readonly SignInCommandValidator _sut = new();

    public static IEnumerable<object[]> GetInvalidCommand()
    {
        var validCommand = new SignInCommand("Qwerty1", "Qwerty2");

        yield return new object[] { validCommand with { Login = string.Empty, Password = string.Empty } };
        yield return new object[] { validCommand with { Password = string.Empty } };
        yield return new object[] { validCommand with { Login = string.Empty } };
        yield return new object[] { validCommand with { Login = "Duo accusam ut. Kasd dolor justo vero sed dolore nonumy tation erat aliquyam labore clita gubergren. Gubergren et consequat amet nonumy diam consequat praesent sed lorem ipsum nostrud illum delenit. Dolores amet feugait ut diam.", } };
    }

    [Fact]
    public void ReturnSuccess_WhenCommandValid()
    {
        var command = new SignInCommand("Qwerty1", "Qwerty2");
        _sut.Validate(command).IsValid.Should().BeTrue();
    }

    [Theory]
    [MemberData(nameof(GetInvalidCommand))]
    public void ReturnFailure_WhenCommandInvalid(SignInCommand command)
    {
        _sut.Validate(command).IsValid.Should().BeFalse();
    }
}