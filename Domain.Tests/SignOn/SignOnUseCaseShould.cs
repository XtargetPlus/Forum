using Domain.Authentication;
using Domain.UseCases.SignOn;
using FluentAssertions;
using Moq;
using Moq.Language.Flow;

namespace Domain.Tests.SignOn;

public class SignOnUseCaseShould
{
    private readonly SignOnUseCase _sut;
    private readonly ISetup<IPasswordManager, (byte[] salt, byte[] hash)> _generatePasswordPartsSetup;
    private readonly ISetup<ISignOnStorage, Task<Guid>> _createUserSetup;
    private readonly Mock<ISignOnStorage> _storage;

    public SignOnUseCaseShould()
    {
        var passwordManager = new Mock<IPasswordManager>();
        _generatePasswordPartsSetup = passwordManager
            .Setup(m => m.GeneratePasswordParts(It.IsAny<string>()));

        _storage = new Mock<ISignOnStorage>();
        _createUserSetup = _storage.Setup(s => s.CreateUser(It.IsAny<string>(), It.IsAny<byte[]>(), It.IsAny<byte[]>(), It.IsAny<CancellationToken>()));

        _sut = new SignOnUseCase(passwordManager.Object, _storage.Object);
    }

    [Fact]
    public async Task CreateUser_WithGeneratedPasswordParts()
    {
        var salt = new byte[] { 1 };
        var hash = new byte[] { 2 };
        _generatePasswordPartsSetup.Returns((salt, hash));

        await _sut.Handle(new SignOnCommand("Qwerty1", "Qwerty2"), CancellationToken.None);

        _storage.Verify(s => s.CreateUser("Qwerty1", salt, hash, CancellationToken.None), Times.Once);
        _storage.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task ReturnIdentityOfNewlyCreatedUser()
    {
        _generatePasswordPartsSetup.Returns((new byte[] { 1 }, new byte[] { 2 }));
        
        var userId = "f2f2d263-aa93-4012-b756-ef3042f91cd8";
        _createUserSetup.ReturnsAsync(Guid.Parse(userId));

        var actual = await _sut.Handle(new SignOnCommand("Qwerty1", "Qwerty2"), CancellationToken.None);
        actual.UserId.Should().Be(userId);
    }
}