using Domain.Authentication;
using FluentValidation;
using MediatR;

namespace Domain.UseCases.SignOn;

internal class SignOnUseCase(
        IPasswordManager passwordManager,
        ISignOnStorage storage,
        IValidator<SignOnCommand> validator)
    : IRequestHandler<SignOnCommand, IIdentity>
{
    public async Task<IIdentity> Handle(SignOnCommand command, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(command, cancellationToken);

        var (salt, hash) = passwordManager.GeneratePasswordParts(command.Password);
        var userId = await storage.CreateUser(command.Login, salt, hash, cancellationToken);

        return new Identity(userId, Guid.Empty);
    }
}