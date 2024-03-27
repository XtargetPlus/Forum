using Domain.Authentication;
using Domain.Authorization;
using FluentValidation;
using MediatR;

namespace Domain.UseCases.SignOut;

internal class SignOutUseCase(
        IIntentionManager intentionManager,
        IIdentityProvider identityProvider,
        IValidator<SignOutCommand> validator,
        ISignOutStorage storage)
    : IRequestHandler<SignOutCommand>   
{
    public async Task Handle(SignOutCommand command, CancellationToken cancellationToken)
    {
        intentionManager.ThrowIfForbidden(AccountIntention.SignOut);
        await validator.ValidateAsync(command, cancellationToken);

        var sessionId = identityProvider.Current.SessionId;
        await storage.RemoveSession(sessionId, cancellationToken);
    }
}