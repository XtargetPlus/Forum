using Search.Domain.Dtos;

namespace Search.Domain.UseCases.Index;

public interface IIndexStorage
{
    Task Index(Guid entityId, SearchEntityType entityType, string? title, string? text, CancellationToken cancellationToken);
}