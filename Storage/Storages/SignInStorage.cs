using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Dtos;
using Domain.UseCases.SignIn;
using Microsoft.EntityFrameworkCore;
using Storage.Models;

namespace Storage.Storages;

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