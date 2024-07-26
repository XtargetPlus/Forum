using AutoMapper;
using Forum.API.Models.Requests;
using Forum.API.Models.Responses;
using Forum.Domain.UseCases.CreateForum;
using Forum.Domain.UseCases.CreateTopic;
using Forum.Domain.UseCases.GetForums;
using Forum.Domain.UseCases.GetTopics;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Forum.API.Controllers;

[ApiController]
[Route("api/forums")]
public class ForumController(IMapper dataMapper, IMediator mediator) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(403)]
    [ProducesResponseType(400)]
    [ProducesResponseType(201, Type = typeof(ForumResponse))]
    public async Task<IActionResult> CreateForum(
        [FromBody] CreateForumRequest request,
        CancellationToken cancellationToken)
    {
        var command = new CreateForumCommand(request.Title);
        var forum = await mediator.Send(command, cancellationToken);

        return CreatedAtRoute(nameof(GetForums), dataMapper.Map<ForumResponse>(forum));
    }

    [HttpPost("{forumId:guid}/topics")]
    [ProducesResponseType(403)]
    [ProducesResponseType(410)]
    [ProducesResponseType(400)]
    [ProducesResponseType(201, Type = typeof(TopicResponse))]
    public async Task<IActionResult> CreateTopic(
        Guid forumId,
        [FromBody] CreateTopicRequest request,
        CancellationToken cancellationToken)
    {
        var topic = await mediator.Send(new CreateTopicCommand(forumId, request.Title), cancellationToken);

        return CreatedAtRoute(nameof(GetForums), dataMapper.Map<TopicResponse>(topic));
    }

    [HttpGet(Name = nameof(GetForums))]
    [ProducesResponseType(200, Type = typeof(ForumResponse))]
    public async Task<IActionResult> GetForums(
        CancellationToken cancellationToken)
    {
        var forums = await mediator.Send(new GetForumsQuery(), cancellationToken);
        return Ok(dataMapper.Map<IEnumerable<ForumResponse>>(forums));
    }

    [HttpGet("{forumId:guid}/topics")]
    [ProducesResponseType(400)]
    [ProducesResponseType(410)]
    [ProducesResponseType(200)]
    public async Task<IActionResult> GetTopics(
        Guid forumId,
        [FromQuery] int skip,
        [FromQuery] int take,
        CancellationToken cancellationToken)
    {
        var query = new GetTopicsQuery(forumId, skip, take);
        var (resources, totalCount) = await mediator.Send(query, cancellationToken);

        return Ok(new { resources = dataMapper.Map<IEnumerable<TopicResponse>>(resources), totalCount });
    }
}