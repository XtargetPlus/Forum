﻿using Domain.UseCases.SignOut;

namespace Storage.Storages;

internal class SignOutStorage : ISignOutStorage
{
    public Task RemoveSession(Guid sessionId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}