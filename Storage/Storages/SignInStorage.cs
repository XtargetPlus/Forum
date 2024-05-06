using AutoMapper;
using AutoMapper.QueryableExtensions;
using Forum.Domain.Dtos;
using Forum.Domain.UseCases.SignIn;
using Forum.Storage.Models;
using Microsoft.EntityFrameworkCore;

namespace Forum.Storage.Storages;

internal class SignInStorage(
        IMapper dataMapper,
        AppDbContext dbContext)
    : ISignInStorage
{
    public Task<RecognizedUser?> FindUser(string login, CancellationToken cancellationToken) => dbContext.Users
        .Where(u => u.Login.Equals(login))
        .ProjectTo<RecognizedUser>(dataMapper.ConfigurationProvider)
        .FirstOrDefaultAsync(cancellationToken);

    public async Task<Guid> CreateSession(Guid userId, DateTimeOffset expirationMoment, CancellationToken cancellationToken)
    {
        var session = new Session
        {
            UserId = userId,
            ExpiresAt = expirationMoment
        };
        await dbContext.Sessions.AddAsync(session, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return session.Id;
    }
}