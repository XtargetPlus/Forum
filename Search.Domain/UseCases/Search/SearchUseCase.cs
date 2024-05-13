using MediatR;
using Search.Domain.Dtos;

namespace Search.Domain.UseCases.Search;

internal class SearchUseCase(ISearchStorage storage) : IRequestHandler<SearchQuery, (IEnumerable<SearchResult> resources, int totalCount)>
{
    public Task<(IEnumerable<SearchResult> resources, int totalCount)> Handle(SearchQuery query, CancellationToken cancellationToken) =>
        storage.Search(query.Query, cancellationToken);
}