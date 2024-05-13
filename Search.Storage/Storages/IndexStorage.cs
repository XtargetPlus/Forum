using OpenSearch.Client;
using Search.Domain.Dtos;
using Search.Domain.UseCases.Index;
using Search.Storage.Models;

namespace Search.Storage.Storages;

internal class IndexStorage(IOpenSearchClient client) : IIndexStorage
{
    public Task Index(Guid entityId, SearchEntityType entityType, string? title, string? text, CancellationToken cancellationToken)
    {
        client.IndexAsync(new SearchEntity
        {
            EntityId = entityId,
            EntityType = (int)entityType,
            Title = title,
            Text = text
        }, descriptor => descriptor.Index("forum-search-v1"), cancellationToken);
        
        return Task.CompletedTask;
    }
}