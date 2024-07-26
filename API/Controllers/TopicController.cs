using AutoMapper;
using Forum.API.Models.Requests;
using Forum.API.Models.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Forum.Domain.UseCases.CreateComment;
using Forum.Domain.UseCases.GetComments;

namespace Forum.API.Controllers;

[ApiController]
[Route("api/forums/topics")]
public class TopicController(IMapper dataMapper, IMediator mediator) : ControllerBase
{
    [HttpPost("{topicId:guid}/comments")]
    [ProducesResponseType(403)]
    [ProducesResponseType(400)]
    [ProducesResponseType(201, Type = typeof(CommentResponse))]
    public async Task<IActionResult> CreateComment(
        Guid topicId, 
        [FromBody] CreateCommentRequest request,
        CancellationToken cancellationToken)
    {
        var command = new CreateCommentCommand(topicId, request.Text);
        var comment = await mediator.Send(command, cancellationToken);

        return CreatedAtRoute(nameof(GetComments), new { topicId }, dataMapper.Map<CommentResponse>(comment));
    }

    [HttpGet("{topicId:guid}/comments", Name = nameof(GetComments))]
    [ProducesResponseType(400)]
    [ProducesResponseType(410)]
    [ProducesResponseType(200)]
    public async Task<IActionResult> GetComments(
        Guid topicId,
        [FromQuery] int skip,
        [FromQuery] int take,
        CancellationToken cancellationToken)
    {
        var query = new GetCommentsQuery(topicId, skip, take);
        var (resources, totalCount) = await mediator.Send(query, cancellationToken);

        return Ok(new { resources = dataMapper.Map<IEnumerable<CommentResponse>>(resources), totalCount });
    }
}