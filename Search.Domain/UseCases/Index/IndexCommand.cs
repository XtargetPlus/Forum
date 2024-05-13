using MediatR;
using Search.Domain.Dtos;

namespace Search.Domain.UseCases.Index;

public record IndexCommand(Guid EntityId, SearchEntityType EntityType, string? Title, string? Text) : IRequest;