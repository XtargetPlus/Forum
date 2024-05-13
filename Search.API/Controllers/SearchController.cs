using MediatR;
using Microsoft.AspNetCore.Mvc;
using Search.Domain.Dtos;
using Search.Domain.UseCases.Index;
using Search.Domain.UseCases.Search;

namespace Search.API.Controllers;

public class SearchController(IMediator mediator) : ControllerBase
{
    [HttpPost("index")]
    public async Task<IActionResult> Index([FromBody] SearchEntityDto dto, CancellationToken cancellationToken)
    {
        var command = new IndexCommand(dto.EntityId, dto.EntityType, dto.Title, dto.Text);
        await mediator.Send(command, cancellationToken);

        return Ok();
    }

    [HttpGet("search")]
    public async Task<IActionResult> Search(string query, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new SearchQuery(query), cancellationToken);

        return Ok(result);
    }
}