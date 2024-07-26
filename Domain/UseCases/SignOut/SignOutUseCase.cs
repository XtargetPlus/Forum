using Forum.Domain.Authentication;
using Forum.Domain.Authorization;
using Forum.Domain.Authorization.AccessManagement;
using MediatR;

namespace Forum.Domain.UseCases.SignOut;

internal class SignOutUseCase(
        IIntentionManager intentionManager,
        IIdentityProvider identityProvider,
        ISignOutStorage storage)
    : IRequestHandler<SignOutCommand>
{
    public async Task Handle(SignOutCommand command, CancellationToken cancellationToken)
    {
        intentionManager.ThrowIfForbidden(AccountIntention.SignOut);

        var sessionId = identityProvider.Current.SessionId;
        await storage.RemoveSession(sessionId, cancellationToken);
    }
}