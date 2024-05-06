using Forum.Domain.Dtos;

namespace Forum.Domain.Authentication;

public interface IAuthenticationStorage
{
    Task<SessionDto?> FindSession(Guid sessionId, CancellationToken cancellationToken);
}