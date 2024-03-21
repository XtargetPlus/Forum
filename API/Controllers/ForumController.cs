using API.Dtos.Requests;
using API.Dtos.Responses;
using AutoMapper;
using Domain.Dtos;
using Domain.UseCases.CreateForum;
using Domain.UseCases.CreateTopic;
using Domain.UseCases.GetForums;
using Domain.UseCases.GetTopics;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/forums")]
public class ForumController(IMapper dataMapper) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(403)]
    [ProducesResponseType(400)]
    [ProducesResponseType(201, Type = typeof(ForumResponse))]
    public async Task<IActionResult> CreateForum(
        [FromBody] CreateForumRequest request,
        ICreateForumUseCase useCase,
        CancellationToken cancellationToken)
    {
        var command = new CreateForumCommand(request.Title);
        var forum = await useCase.Execute(command, cancellationToken);

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
        ICreateTopicUseCase useCase,
        CancellationToken cancellationToken)
    {
        var topic = await useCase.Execute(new CreateTopicCommand(forumId, request.Title), cancellationToken);

        return CreatedAtRoute(nameof(GetForums), dataMapper.Map<TopicResponse>(topic));
    }

    [HttpGet(Name = nameof(GetForums))]
    [ProducesResponseType(200, Type = typeof(ForumResponse))]
    public async Task<ActionResult<ForumResponse>> GetForums(
        IGetForumsUseCase userCase,
        CancellationToken cancellationToken)
    {
        var forums = await userCase.Execute(cancellationToken);
        return Ok(dataMapper.Map<IEnumerable<ForumResponse>>(forums));
    }

    [HttpGet("{forumId:guid}/topics")]
    [ProducesResponseType(400)]
    [ProducesResponseType(410)]
    [ProducesResponseType(200)]
    public async Task<ActionResult> GetTopics(
        Guid forumId,
        [FromQuery] int skip,
        [FromQuery] int take,
        IGetTopicsUseCase useCase,
        CancellationToken cancellation)
    {
        var query = new GetTopicsQuery(forumId, skip, take);
        var (resources, totalCount) = await useCase.Execute(query, cancellation);

        return Ok(new { resources = dataMapper.Map<IEnumerable<TopicResponse>>(resources), totalCount });
    }
}