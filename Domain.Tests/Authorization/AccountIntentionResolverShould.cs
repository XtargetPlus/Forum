using FluentAssertions;
using Forum.Domain.Authentication;
using Forum.Domain.Authorization;
using Moq;

namespace Forum.Domain.Tests.Authorization;

public class AccountIntentionResolverShould
{
    private readonly AccountIntentionResolver _sut = new();

    [Fact]
    public void ReturnFalse_WhenIntentionNotInEnum()
    {
        var intention = (AccountIntention)(-1);

        _sut.IsAllowed(new Mock<IIdentity>().Object, intention).Should().BeFalse();
    }

    [Fact]
    public void ReturnFalse_WheCheckingForumCreateIntention_AndUserIsGuest()
    {
        _sut.IsAllowed(Identity.Guest, AccountIntention.SignOut).Should().BeFalse();
    }

    [Fact]
    public void ReturnTrue_WhenCheckingForumCreateIntention_AndUserIsAuthenticated()
    {
        _sut.IsAllowed(new Identity(Guid.Parse("401d84d1-abb9-45a4-90a0-50b11f64bd46"), Guid.Empty), AccountIntention.SignOut).Should().BeTrue();
    }
}