using Domain.Dtos;

namespace Domain.Authentication;

public interface IAuthenticationStorage
{
    Task<SessionDto?> FindSession(Guid sessionId, CancellationToken cancellationToken);
}