using Domain.Dtos;

namespace Domain.UseCases.SignIn;

public interface ISignInStorage
{
    Task<RecognizedUser?> FindUser(string login, CancellationToken cancellationToken);
    Task<Guid> CreateSession(Guid userId, DateTimeOffset expirationMoment, CancellationToken cancellationToken);
}
