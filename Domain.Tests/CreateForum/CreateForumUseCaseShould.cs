using FluentAssertions;
using Forum.Domain.Authorization;
using Forum.Domain.Dtos;
using Forum.Domain.UseCases.CreateForum;
using Moq;
using Moq.Language.Flow;

namespace Forum.Domain.Tests.CreateForum;

public class CreateForumUseCaseShould
{
    private readonly CreateForumUseCase _sut;
    private readonly Mock<ICreateForumStorage> _storage;
    private readonly ISetup<ICreateForumStorage, Task<ForumDto>> _createForumSetup;

    public CreateForumUseCaseShould()
    {
        var intentionManager = new Mock<IIntentionManager>();
        intentionManager
            .Setup(m => m.IsAllowed(It.IsAny<ForumIntention>()))
            .Returns(true);

        _storage = new Mock<ICreateForumStorage>();
        _createForumSetup = _storage.Setup(s => s.CreateForum(It.IsAny<CreateForumCommand>(), It.IsAny<CancellationToken>()));

        _sut = new CreateForumUseCase(intentionManager.Object, _storage.Object);
    }

    [Fact]
    public async Task ReturnCreatedForum()
    {
        const string forumTitle = "Test";
        var forum = new ForumDto
        {
            ForumId = Guid.Parse("6963bbaa-5482-44a2-a205-fa46871fccc4"),
            Title = forumTitle
        };

        _createForumSetup.ReturnsAsync(forum);

        var actual = await _sut.Handle(new CreateForumCommand(forumTitle), CancellationToken.None);
        actual.Should().BeEquivalentTo(forum);

        _storage.Verify(s => s.CreateForum(new CreateForumCommand(forumTitle), It.IsAny<CancellationToken>()), Times.Once);
        _storage.VerifyNoOtherCalls();
    }
}