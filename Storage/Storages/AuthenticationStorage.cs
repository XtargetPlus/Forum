using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Authentication;
using Domain.Dtos;
using Microsoft.EntityFrameworkCore;

namespace Storage.Storages;

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