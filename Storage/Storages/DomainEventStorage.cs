using System.Text.Json;
using Forum.Domain.UseCases;
using Forum.Storage.Models;

namespace Forum.Storage.Storages;

internal class DomainEventStorage(AppDbContext dbContext, TimeProvider timeProvider) : IDomainEventStorage
{
    public async Task AddEvent<TDomainEntity>(TDomainEntity entity, CancellationToken cancellationToken)
    {
        await dbContext.DomainEvents.AddAsync(new DomainEvent
        {
            EmittedAt = timeProvider.GetUtcNow(),
            ContentBlob = JsonSerializer.SerializeToUtf8Bytes(entity)
        }, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}