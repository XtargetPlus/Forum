﻿using Domain.Dtos;
using Domain.Exceptions;
using Domain.UseCases.GetForums;
using Domain.UseCases.GetTopics;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using Moq.Language.Flow;

namespace Domain.Tests.GetTopics;

public class GetTopicsUseCaseShould
{
    private readonly GetTopicsUseCase _sut;
    private readonly Mock<IGetTopicsStorage> _storage;
    private readonly ISetup<IGetTopicsStorage, Task<(IEnumerable<TopicDto> resources, int totalCount)>> _getTopicsSetup;
    private readonly ISetup<IGetForumsStorage, Task<IEnumerable<ForumDto>>> _getForumsSetup;

    public GetTopicsUseCaseShould()
    {
        var validator = new Mock<IValidator<GetTopicsQuery>>();
        validator
            .Setup(v => v.ValidateAsync(It.IsAny<GetTopicsQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        _storage = new Mock<IGetTopicsStorage>();
        var forumsStorage = new Mock<IGetForumsStorage>();
        
        _getTopicsSetup = _storage.Setup(s => s.GetTopics(It.IsAny<GetTopicsQuery>(), It.IsAny<CancellationToken>()));
        _getForumsSetup = forumsStorage.Setup(s => s.GetForums(It.IsAny<CancellationToken>()));

        _sut = new GetTopicsUseCase(_storage.Object, forumsStorage.Object, validator.Object);
    }

    [Fact]
    public async Task ThrowForumNotFoundException_WhenNoForum()
    {
        var forumId = Guid.Parse("629d37ee-6259-4546-b7d3-ff30465dbf9e");

        _getForumsSetup.ReturnsAsync(new ForumDto[] { new() { ForumId = Guid.Parse("7bceae9c-3733-47ea-b4cb-06c8907fce7a") } });

        var query = new GetTopicsQuery(forumId, 5, 10);
        await _sut.Invoking(s => s.Execute(query, CancellationToken.None)).Should().ThrowAsync<ForumNotFoundException>();
    }

    [Fact]
    public async Task ReturnTopics_ExtractedFromStorage_WhenForumExists()
    {
        var forumId = Guid.Parse("8c8c5714-8122-43ca-ae2d-139dec1f0e0c");

        var expectedResources = new TopicDto[] { new() };
        var expectedTotalCount = 6;
        _getTopicsSetup.ReturnsAsync((expectedResources, expectedTotalCount));
        _getForumsSetup.ReturnsAsync(new ForumDto[] { new() { ForumId = forumId } });

        var (actualResources, actualTotalCount) = await _sut.Execute(new GetTopicsQuery(forumId, 5, 10), CancellationToken.None);

        actualResources.Should().BeEquivalentTo(expectedResources);
        actualTotalCount.Should().Be(expectedTotalCount);
        _storage.Verify(s => s.GetTopics(It.IsAny<GetTopicsQuery>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}