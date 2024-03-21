using Domain.Authentication;

namespace Domain.UseCases.SignOn;

public interface ISignOnUseCase
{
    Task<IIdentity> Execute(SignOnCommand command, CancellationToken cancellationToken);
}