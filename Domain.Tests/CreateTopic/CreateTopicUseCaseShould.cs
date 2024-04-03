using Domain.Authentication;
using Domain.Authorization;
using Domain.Dtos;
using Domain.Exceptions;
using Domain.UseCases.CreateTopic;
using Domain.UseCases.GetForums;
using FluentAssertions;
using Moq;
using Moq.Language.Flow;
using CreateTopicCommand = Domain.UseCases.CreateTopic.CreateTopicCommand;
using IIdentity = Domain.Authentication.IIdentity;

namespace Domain.Tests.CreateTopic;

public class CreateTopicUseCaseShould
{
    private readonly CreateTopicUseCase _sut;
    private readonly ISetup<ICreateTopicStorage, Task<TopicDto>> _createTopicSetup;
    private readonly Mock<ICreateTopicStorage> _storage;
    private readonly ISetup<IIdentity, Guid> _getCurrentUserIdSetup;
    private readonly ISetup<IIntentionManager, bool> _intentionIsAllowedSetup;
    private readonly Mock<IIntentionManager> _intentionManager;
    private readonly ISetup<IGetForumsStorage, Task<IEnumerable<ForumDto>>> _getForumsSetup;

    public CreateTopicUseCaseShould()
    {
        _storage = new Mock<ICreateTopicStorage>();
        Mock<IGetForumsStorage> getForumsStorage = new();

        _createTopicSetup = _storage.Setup(s => s.CreateTopic(It.IsAny<CreateTopicCommand>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()));
        _getForumsSetup = getForumsStorage.Setup(s => s.GetForums(It.IsAny<CancellationToken>()));

        var identity = new Mock<IIdentity>();
        var identityProvider = new Mock<IIdentityProvider>();
        identityProvider.Setup(p => p.Current).Returns(identity.Object);
        _getCurrentUserIdSetup = identity.Setup(s => s.UserId);

        _intentionManager = new Mock<IIntentionManager>();
        _intentionIsAllowedSetup = _intentionManager.Setup(m => m.IsAllowed(It.IsAny<TopicIntention>()));

        _sut = new CreateTopicUseCase(_intentionManager.Object, _storage.Object, getForumsStorage.Object, identityProvider.Object);
    }

    [Fact]
    public async Task ThrowIntentionManagerException_WhenTopicCreationIsNotAllowed()
    {
        var forumId = Guid.Parse("1adbb258-866f-4991-bd4f-3232e0d28f50");

        _intentionIsAllowedSetup.Returns(false);
        await _sut.Invoking(s => s.Handle(new CreateTopicCommand(forumId, "Something"), CancellationToken.None))
            .Should().ThrowAsync<IntentionManagerException>();
        _intentionManager.Verify(m => m.IsAllowed(TopicIntention.Create));
    }

    [Fact]
    public async Task ThrowForumNotFoundException_WhenMatchingForumExists()
    {
        var forumId = Guid.Parse("747eb84c-4868-435f-a80b-0b16f1f8c820");

        _intentionIsAllowedSetup.Returns(true);
        _getForumsSetup.ReturnsAsync(Array.Empty<ForumDto>());

        (await _sut.Invoking(s => s.Handle(new CreateTopicCommand(forumId, "Some title"), CancellationToken.None))
            .Should().ThrowAsync<ForumNotFoundException>())
            .Which.ErrorCode.Should().Be(DomainErrorCode.Gone410);
    }

    [Fact]
    public async Task ReturnNewlyCreatedTopic()
    {
        var forumId = Guid.Parse("0a90bdf6-5d3f-4ec2-8fdb-e427dfba32c4");
        var userId = Guid.Parse("357942e5-4cee-422e-8247-afed62c5d0a1");

        var expected = new TopicDto();
        _createTopicSetup.ReturnsAsync(expected);
        _intentionIsAllowedSetup.Returns(true);
        _getForumsSetup.ReturnsAsync(new ForumDto[] { new() { ForumId = forumId} });
        _getCurrentUserIdSetup.Returns(userId);

        var newTopic = new CreateTopicCommand(forumId, "Hello World");

        var actual = await _sut.Handle(newTopic, CancellationToken.None);

        actual.Should().BeEquivalentTo(expected);
        _storage.Verify(s => s.CreateTopic(newTopic, userId, It.IsAny<CancellationToken>()));
    }
}