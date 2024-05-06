using AutoMapper;
using AutoMapper.QueryableExtensions;
using Forum.Domain.Authentication;
using Forum.Domain.Dtos;
using Microsoft.EntityFrameworkCore;

namespace Forum.Storage.Storages;

internal class AuthenticationStorage(
        AppDbContext dbContext,
        IMapper dataMapper)
    : IAuthenticationStorage
{
    public Task<SessionDto?> FindSession(Guid sessionId, CancellationToken cancellationToken) => dbContext.Sessions
        .Where(s => s.Id == sessionId)
        .ProjectTo<SessionDto>(dataMapper.ConfigurationProvider)
        .FirstOrDefaultAsync(cancellationToken);
}