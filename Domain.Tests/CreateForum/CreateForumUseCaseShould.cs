using Domain.Authorization;
using Domain.Dtos;
using Domain.UseCases.CreateForum;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using Moq.Language.Flow;

namespace Domain.Tests.CreateForum;

public class CreateForumUseCaseShould
{
    private readonly CreateForumUseCase _sut;
    private readonly Mock<ICreateForumStorage> _storage;
    private readonly ISetup<ICreateForumStorage, Task<ForumDto>> _createForumSetup;

    public CreateForumUseCaseShould()
    {
        var validator = new Mock<IValidator<CreateForumCommand>>();
        validator
            .Setup(v => v.ValidateAsync(It.IsAny<CreateForumCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        var intentionManager = new Mock<IIntentionManager>();
        intentionManager
            .Setup(m => m.IsAllowed(It.IsAny<ForumIntention>()))
            .Returns(true);

        _storage = new Mock<ICreateForumStorage>(); 
        _createForumSetup = _storage.Setup(s => s.CreateForum(It.IsAny<CreateForumCommand>(), It.IsAny<CancellationToken>()));

        _sut = new CreateForumUseCase(validator.Object, intentionManager.Object, _storage.Object);
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

        var actual = await _sut.Execute(new CreateForumCommand(forumTitle), CancellationToken.None);
        actual.Should().BeEquivalentTo(forum);

        _storage.Verify(s => s.CreateForum(new CreateForumCommand(forumTitle), It.IsAny<CancellationToken>()), Times.Once);
        _storage.VerifyNoOtherCalls();
    }
}