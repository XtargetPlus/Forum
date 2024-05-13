using MediatR;

namespace Search.Domain.UseCases.Index;

internal class IndexUseCase(IIndexStorage storage) : IRequestHandler<IndexCommand>
{
    public Task Handle(IndexCommand command, CancellationToken cancellationToken)
    {
        var (entityId, entityType, title, text) = command;
        return storage.Index(entityId, entityType, title, text, cancellationToken);
    }
}