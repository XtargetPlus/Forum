using System.Security.Cryptography;
using FluentAssertions;
using Forum.Domain.Authentication;
using Forum.Domain.Dtos;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Language.Flow;

namespace Forum.Domain.Tests.Authentication;

public class AuthenticationServiceShould
{
    private readonly AuthenticationService _sut;
    private readonly ISetup<ISymmetricDecryptor, Task<string>> _setupDecryptor;
    private readonly ISetup<IAuthenticationStorage, Task<SessionDto?>> _findUserIdSetup;

    public AuthenticationServiceShould()
    {
        var decryptor = new Mock<ISymmetricDecryptor>();
        _setupDecryptor = decryptor.Setup(d => d.Decrypt(It.IsAny<string>(), It.IsAny<byte[]>(), It.IsAny<CancellationToken>()));

        var options = new Mock<IOptions<AuthenticationConfiguration>>();
        options
            .Setup(o => o.Value)
            .Returns(new AuthenticationConfiguration
            {
                Base64Key = "FWsfgLeTrXAJKcKrDWcgAYf+PQfaUbYynFwoL9DnOd0="
            });

        var storage = new Mock<IAuthenticationStorage>();
        _findUserIdSetup = storage.Setup(s => s.FindSession(It.IsAny<Guid>(), It.IsAny<CancellationToken>()));

        _sut = new AuthenticationService(
            NullLogger<AuthenticationService>.Instance,
            storage.Object,
            decryptor.Object,
            options.Object);
    }

    [Fact]
    public async Task ReturnGuestIdentity_WhenTokenCannotBeDecrypted()
    {
        _setupDecryptor.Throws<CryptographicException>();
        var actual = await _sut.Authenticate("shit-bad-token", CancellationToken.None);

        actual.Should().BeEquivalentTo(Identity.Guest);
    }

    [Fact]
    public async Task ReturnGuestIdentity_WhenTokenIsInvalid()
    {
        _setupDecryptor.ReturnsAsync("aboba");
        var actual = await _sut.Authenticate("aboba-bad-token", CancellationToken.None);

        actual.Should().BeEquivalentTo(Identity.Guest);
    }

    [Fact]
    public async Task ReturnGuestIdentity_WhenTokenIsExpired()
    {
        _setupDecryptor.ReturnsAsync("d9abc555-8f06-4786-a33a-037074e203d7");
        _findUserIdSetup.ReturnsAsync(new SessionDto { ExpiresAt = DateTimeOffset.UtcNow.AddDays(-1) });

        var actual = await _sut.Authenticate("shit-expired-token", CancellationToken.None);

        actual.Should().BeEquivalentTo(Identity.Guest);
    }

    [Fact]
    public async Task ReturnGuestIdentity_WhenSessionNotFound()
    {
        _setupDecryptor.ReturnsAsync("d9abc555-8f06-4786-a33a-037074e203d7");
        _findUserIdSetup.ReturnsAsync(() => null);

        var actual = await _sut.Authenticate("shit-good-token", CancellationToken.None);

        actual.Should().BeEquivalentTo(Identity.Guest);
    }

    [Fact]
    public async Task ReturnIdentity_WhenSessionIsValid()
    {
        var userId = "d9abc555-8f06-4786-a33a-037074e203d7";
        var sessionId = "8f5f7e8f-e73c-445d-b932-3b44b08c0b21";

        _setupDecryptor.ReturnsAsync(sessionId);
        _findUserIdSetup.ReturnsAsync(new SessionDto
        {
            UserId = Guid.Parse(userId),
            ExpiresAt = DateTimeOffset.UtcNow.AddDays(1)
        });

        var actual = await _sut.Authenticate("shit-good-token", CancellationToken.None);

        actual.Should().BeEquivalentTo(new Identity(Guid.Parse(userId), Guid.Parse(sessionId)));
    }
}