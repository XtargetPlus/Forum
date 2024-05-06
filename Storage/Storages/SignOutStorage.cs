using Forum.Domain.UseCases.SignOut;

namespace Forum.Storage.Storages;

internal class SignOutStorage : ISignOutStorage
{
    public Task RemoveSession(Guid sessionId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}