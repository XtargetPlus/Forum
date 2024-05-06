using FluentAssertions;
using Forum.Domain.Authentication;

namespace Forum.Domain.Tests.Authentication;

public class PasswordManagerShould
{
    private readonly PasswordManager _sut = new();
    private static readonly byte[] EmptySalt = Enumerable.Repeat((byte)0, 100).ToArray();
    private static readonly byte[] EmptyHash = Enumerable.Repeat((byte)0, 32).ToArray();

    [Theory]
    [InlineData("password")]
    [InlineData("qwerty")]
    public void GenerateMeaningfulSaltAndHash(string password)
    {
        var (salt, hash) = _sut.GeneratePasswordParts(password);
        salt.Should().HaveCount(100).And.NotBeEquivalentTo(EmptySalt);
        hash.Should().HaveCount(32).And.NotBeEquivalentTo(EmptyHash);
    }

    [Fact]
    public void ReturnTrue_WhenPasswordMatch()
    {
        var password = "qwerty";
        var (salt, hash) = _sut.GeneratePasswordParts(password);
        _sut.ComparePasswords(password, salt, hash).Should().BeTrue();
    }

    [Fact]
    public void ReturnFalse_WhenPasswordDoesntMatch()
    {
        var password = "qwerty";
        var invalidPassword = "ytrewq";
        var (salt, hash) = _sut.GeneratePasswordParts(password);
        _sut.ComparePasswords(invalidPassword, salt, hash).Should().BeFalse();
    }
}