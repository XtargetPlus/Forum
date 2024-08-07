﻿using FluentAssertions;
using Forum.Domain.Authentication;
using Forum.Domain.Authorization;
using Forum.Domain.Authorization.AccessManagement;
using Forum.Domain.Exceptions;
using Forum.Domain.UseCases.SignOut;
using Moq;
using Moq.Language.Flow;

namespace Forum.Domain.Tests.SignOut;

public class SignOutUseCaseShould
{
    private readonly SignOutUseCase _sut;
    private readonly Mock<ISignOutStorage> _storage;
    private readonly ISetup<ISignOutStorage, Task> _removeSessionSetup;
    private readonly ISetup<IIdentityProvider, IIdentity> _currentIdentitySetup;
    private readonly ISetup<IIntentionManager, bool> _signOutIsAllowedSetup;

    public SignOutUseCaseShould()
    {
        _storage = new Mock<ISignOutStorage>();
        _removeSessionSetup = _storage.Setup(s => s.RemoveSession(It.IsAny<Guid>(), It.IsAny<CancellationToken>()));

        var identityProvider = new Mock<IIdentityProvider>();
        _currentIdentitySetup = identityProvider.Setup(p => p.Current);

        var intentionManager = new Mock<IIntentionManager>();
        _signOutIsAllowedSetup = intentionManager.Setup(m => m.IsAllowed(It.IsAny<AccountIntention>()));

        _sut = new SignOutUseCase(
            intentionManager.Object,
            identityProvider.Object,
            _storage.Object);
    }

    [Fact]
    public async Task ThrowIntentionManagerException_WhenUserIsNotAuthenticated()
    {
        _signOutIsAllowedSetup.Returns(false);

        await _sut.Invoking(s => s.Handle(new SignOutCommand(), CancellationToken.None))
            .Should().ThrowAsync<IntentionManagerException>();
    }

    [Fact]
    public async Task RemoveCurrentIdentitySession()
    {
        var sessionId = Guid.Parse("14a07917-bb59-4c5a-9afe-4123b0cffd4c");
        _signOutIsAllowedSetup.Returns(true);
        _currentIdentitySetup.Returns(new Identity(Guid.Empty, sessionId));
        _removeSessionSetup.Returns(Task.CompletedTask);

        await _sut.Handle(new SignOutCommand(), CancellationToken.None);

        _storage.Verify(s => s.RemoveSession(sessionId, It.IsAny<CancellationToken>()), Times.Once);
        _storage.VerifyNoOtherCalls();
    }
}