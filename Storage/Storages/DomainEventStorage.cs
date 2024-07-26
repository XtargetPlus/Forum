using System.Diagnostics;
using System.Text.Json;
using AutoMapper;
using Forum.Domain.UseCases;
using Forum.Storage.Entities;
using ForumDomainEvent = Forum.Domain.DomainEvents.ForumDomainEvent;

namespace Forum.Storage.Storages;

internal class DomainEventStorage(
    AppDbContext dbContext, 
    TimeProvider timeProvider,
    IMapper mapper) : IDomainEventStorage
{
    public async Task AddEvent(ForumDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        var storageDomainEvent = mapper.Map<Models.ForumDomainEvent>(domainEvent);

        await dbContext.DomainEvents.AddAsync(new DomainEvent
        {
            EmittedAt = timeProvider.GetUtcNow(),
            ContentBlob = JsonSerializer.SerializeToUtf8Bytes(storageDomainEvent),
            ActivityId = Activity.Current?.Id 
        }, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}