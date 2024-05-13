using Microsoft.Extensions.DependencyInjection;
using OpenSearch.Client;
using Search.Domain.UseCases.Index;
using Search.Domain.UseCases.Search;
using Search.Storage.Models;
using Search.Storage.Storages;

namespace Search.Storage.DependencyInjection;

public static class SearchCollectionExtensions
{
    public static IServiceCollection AddSearchStorage(this IServiceCollection services, string connectionString)
    {
        services
            .AddScoped<IIndexStorage, IndexStorage>()
            .AddScoped<ISearchStorage, SearchStorage>();

        services.AddSingleton<IOpenSearchClient>(new OpenSearchClient(new Uri(connectionString))
        {
            ConnectionSettings =
            {
                DefaultIndices = { [typeof(SearchEntity)] = "forum-search-v1" }
            }
        });

        return services;
    } 
}