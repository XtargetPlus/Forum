using Forum.Domain.DomainEvents;

namespace Forum.Domain.UseCases;

public interface IDomainEventStorage : IStorage
{
    Task AddEvent(ForumDomainEvent domainEvent, CancellationToken cancellationToken);
}