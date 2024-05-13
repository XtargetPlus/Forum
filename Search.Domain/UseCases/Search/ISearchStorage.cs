﻿using Search.Domain.Dtos;

namespace Search.Domain.UseCases.Search;

public interface ISearchStorage
{
    Task<(IEnumerable<SearchResult> resources, int totalCount)> Search(string query, CancellationToken cancellationToken);
}