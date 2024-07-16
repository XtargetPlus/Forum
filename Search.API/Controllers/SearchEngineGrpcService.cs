using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using MediatR;
using Search.API.Grpc;
using Search.Domain.UseCases.Index;
using Search.Domain.UseCases.Search;
using SearchEntityType = Search.Domain.Dtos.SearchEntityType;

namespace Search.API.Controllers;

internal class SearchEngineGrpcService(IMediator mediator) : SearchEngine.SearchEngineBase
{
    public override async Task<Empty> Index(IndexRequest request, ServerCallContext context)
    {
        var command = new IndexCommand(
            Guid.Parse(request.Id),
            request.Type switch {
                Grpc.SearchEntityType.ForumTopic => SearchEntityType.ForumTopic,
                Grpc.SearchEntityType.ForumComment => SearchEntityType.ForumComment,
                _ => throw new ArgumentOutOfRangeException()
            }, 
            request.Title,
            request.Text);

        await mediator.Send(command, context.CancellationToken);
        return new Empty();
    }

    public override async Task<SearchResponse> Search(SearchReqeust request, ServerCallContext context)
    {
        var query = new SearchQuery(request.Query);

        var (resources, totalCount) = await mediator.Send(query, context.CancellationToken);
        return new SearchResponse
        {
            Total = totalCount,
            Entities =
            {
                resources.Select(r => new SearchResponse.Types.SearchResultEntity
                {
                    Id = r.EntityId.ToString(),
                    Type = r.EntityType switch
                    {
                        SearchEntityType.ForumTopic => Grpc.SearchEntityType.ForumTopic,
                        SearchEntityType.ForumComment => Grpc.SearchEntityType.ForumComment,
                        _ => Grpc.SearchEntityType.Unknown
                    },
                    Highlights = { r.Highlights }
                })
            }
        };
    }
}
