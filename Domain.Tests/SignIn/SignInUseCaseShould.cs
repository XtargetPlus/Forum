using Domain.Authentication;
using Domain.Dtos;
using Domain.UseCases.SignIn;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Language.Flow;

namespace Domain.Tests.SignIn;

public class SignInUseCaseShould
{
    private readonly SignInUseCase _sut;
    private readonly ISetup<ISymmetricEncryptor, Task<string>> _encryptSetup;
    private readonly ISetup<IPasswordManager, bool> _comparePasswordsSetup;
    private readonly ISetup<ISignInStorage, Task<RecognizedUser?>> _findUserSetup;
    private readonly ISetup<ISignInStorage, Task<Guid>> _createSessionSetup;
    private readonly Mock<ISignInStorage> _storage;
    private readonly Mock<ISymmetricEncryptor> _encryptor;

    public SignInUseCaseShould()
    {
        var options = new Mock<IOptions<AuthenticationConfiguration>>();
        options
            .Setup(o => o.Value)
            .Returns(new AuthenticationConfiguration
            {
                Base64Key = "FWsfgLeTrXAJKcKrDWcgAYf+PQfaUbYynFwoL9DnOd0="
            });

        _encryptor = new Mock<ISymmetricEncryptor>();
        _encryptSetup = _encryptor.Setup(e => e.Encrypt(It.IsAny<string>(), It.IsAny<byte[]>(), It.IsAny<CancellationToken>()));

        var passwordManager = new Mock<IPasswordManager>();
        _comparePasswordsSetup = passwordManager.Setup(m => m.ComparePasswords(It.IsAny<string>(), It.IsAny<byte[]>(), It.IsAny<byte[]>()));

        _storage = new Mock<ISignInStorage>();
        _findUserSetup = _storage.Setup(s => s.FindUser(It.IsAny<string>(), It.IsAny<CancellationToken>()));
        _createSessionSetup = _storage.Setup(s => s.CreateSession(It.IsAny<Guid>(), It.IsAny<DateTimeOffset>(), It.IsAny<CancellationToken>()));

        var validator = new Mock<IValidator<SignInCommand>>();
        validator
            .Setup(v => v.ValidateAsync(It.IsAny<SignInCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        _sut = new SignInUseCase(options.Object, _encryptor.Object, passwordManager.Object, _storage.Object, validator.Object);
    }

    [Fact]
    public async Task CreateSession_WhenPasswordMatches()
    {
        var userId = Guid.Parse("3cd89d7a-4e1b-43fa-ba18-505a564d6281");
        var sessionId = Guid.Parse("77d54468-760d-48e4-85b5-78ccbc782652");

        _findUserSetup.ReturnsAsync(new RecognizedUser { UserId = userId });
        _comparePasswordsSetup.Returns(true);
        _createSessionSetup.ReturnsAsync(sessionId);

        await _sut.Handle(new SignInCommand("Qwerty1", "Qwerty2"), CancellationToken.None);
        _storage.Verify(s => s.CreateSession(userId, It.IsAny<DateTimeOffset>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task ThrowValidationException_WhenUserNotFound()
    {
        _findUserSetup.ReturnsAsync(() => null);
        (await _sut.Invoking(s => s.Handle(new SignInCommand("Qwerty1", "Qwerty2"), CancellationToken.None))
            .Should().ThrowAsync<ValidationException>())
            .Which.Errors.Should().Contain(e => e.PropertyName == nameof(SignInCommand.Login));
    }

    [Fact]
    public async Task ThrowValidationException_WhenPasswordDoesNotMatch()
    {
        _findUserSetup.ReturnsAsync(new RecognizedUser());
        _comparePasswordsSetup.Returns(false);
        (await _sut.Invoking(s => s.Handle(new SignInCommand("Qwerty1", "Qwerty2"), CancellationToken.None))
                .Should().ThrowAsync<ValidationException>())
            .Which.Errors.Should().Contain(e => e.PropertyName == nameof(SignInCommand.Password));
    }

    [Fact]
    public async Task ReturnTokenAndIdentity()
    {
        var userId = Guid.Parse("325dec91-9ffe-4c03-96ec-409b5aaaa044");
        var sessionId = Guid.Parse("77d54468-760d-48e4-85b5-78ccbc782652");

        _findUserSetup.ReturnsAsync(new RecognizedUser
        {
            UserId = userId,
            PasswordHash = new byte[] { 1 },
            Salt = new byte[] { 2 },
        });
        _comparePasswordsSetup.Returns(true);
        _encryptSetup.ReturnsAsync("token");
        _createSessionSetup.ReturnsAsync(sessionId);

        var (identity, token) = await _sut.Handle(new SignInCommand("Qwerty1", "Qwerty2"), CancellationToken.None);
        identity.UserId.Should().Be(userId);
        identity.SessionId.Should().Be(sessionId);
        token.Should().Be("token");
    }

    [Fact]
    public async Task EncryptSessionIdIntoToken()
    {
        var userId = Guid.Parse("3cd89d7a-4e1b-43fa-ba18-505a564d6281");
        var sessionId = Guid.Parse("77d54468-760d-48e4-85b5-78ccbc782652");

        _findUserSetup.ReturnsAsync(new RecognizedUser { UserId = userId });
        _comparePasswordsSetup.Returns(true);
        _createSessionSetup.ReturnsAsync(sessionId);

        await _sut.Handle(new SignInCommand("Qwerty1", "Qwerty2"), CancellationToken.None);
        _encryptor.Verify(s => s.Encrypt(sessionId.ToString(), It.IsAny<byte[]>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}