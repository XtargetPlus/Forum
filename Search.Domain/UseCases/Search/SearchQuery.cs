using MediatR;
using Search.Domain.Dtos;

namespace Search.Domain.UseCases.Search;

public record SearchQuery(string Query) : IRequest<(IEnumerable<SearchResult> resources, int totalCount)>;