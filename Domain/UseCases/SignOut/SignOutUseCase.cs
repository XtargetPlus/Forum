using Domain.Authentication;
using Domain.Authorization;
using FluentValidation;

namespace Domain.UseCases.SignOut;

internal class SignOutUseCase(
        IIntentionManager intentionManager,
        IIdentityProvider identityProvider,
        IValidator<SignOutCommand> validator,
        ISignOutStorage storage)
    : ISignOutUseCase
{
    public async Task Execute(SignOutCommand command, CancellationToken cancellationToken)
    {
        intentionManager.ThrowIfForbidden(AccountIntention.SignOut);
        await validator.ValidateAsync(command, cancellationToken);

        var sessionId = identityProvider.Current.SessionId;
        await storage.RemoveSession(sessionId, cancellationToken);
    }
}