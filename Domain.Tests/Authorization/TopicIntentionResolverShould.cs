using Domain.Authentication;
using Domain.UseCases.CreateTopic;
using FluentAssertions;
using Moq;

namespace Domain.Tests.Authorization;

public class TopicIntentionResolverShould
{
    private readonly TopicIntentionResolver _sut = new();

    [Fact]
    public void ReturnFalse_WhenIntentionNotInEnum()
    {
        var intention = (TopicIntention) (-1);

        _sut.IsAllowed(new Mock<IIdentity>().Object, intention).Should().BeFalse();
    }

    [Fact]
    public void ReturnFalse_WheCheckingTopicCreateIntention_AndUserIsGuest()
    {
        _sut.IsAllowed(Identity.Guest, TopicIntention.Create).Should().BeFalse();
    }

    [Fact]
    public void ReturnTrue_WhenCheckingTopicCreateIntention_AndUserIsAuthenticated()
    {
        _sut.IsAllowed(new Identity(Guid.Parse("a9b623c4-7d51-4ab7-8e02-79c72f2cccb0"), Guid.Empty), TopicIntention.Create).Should().BeTrue();
    }
}