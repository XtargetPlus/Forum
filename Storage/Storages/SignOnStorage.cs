using Domain.UseCases.SignOn;
using Storage.Models;

namespace Storage.Storages;

internal class SignOnStorage(AppDbContext dbContext) : ISignOnStorage
{
    public async Task<Guid> CreateUser(string login, byte[] salt, byte[] hash, CancellationToken cancellationToken)
    {
        var user = new User
        {
            Login = login,
            PasswordHash = hash,
            Salt = salt
        };
        await dbContext.Users.AddAsync(user, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
        
        return user.Id;
    }
}