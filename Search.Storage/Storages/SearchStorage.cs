﻿using OpenSearch.Client;
using Search.Domain.Dtos;
using Search.Domain.UseCases.Search;
using Search.Storage.Models;

namespace Search.Storage.Storages;

internal class SearchStorage(IOpenSearchClient client) : ISearchStorage
{
    public async Task<(IEnumerable<SearchResult> resources, int totalCount)> Search(string query, CancellationToken cancellationToken)
    {
        var searchResponse = await client.SearchAsync<SearchEntity>(descriptor => descriptor
            .Query(q => q
                .Bool(b => b
                    .Should(
                        s => s.Match(m => m
                            .Field(se => se.Title).Query(query)),
                        s => s.Match(m => m
                            .Field(se => se.Text).Query(query).Fuzziness(Fuzziness.EditDistance(1))))))
            .Highlight(h => h
                .Fields(
                    f => f.Field(se => se.Title),
                    f => f.Field(se => se.Text).PreTags("<mark>").PostTags("</mark>"))), cancellationToken);

        return (searchResponse.Hits.Select(hit => new SearchResult
        {
            EntityId = hit.Source.EntityId,
            EntityType = (SearchEntityType)hit.Source.EntityType,
            Highlights = hit.Highlight.Values.SelectMany(v => v).ToArray()
        }), (int)searchResponse.Total);
    }
}
