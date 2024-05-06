using System.Net;
using FluentAssertions;
using Forum.Domain.Authentication;
using Forum.Domain.Authorization;
using Forum.Domain.Exceptions;
using Forum.Domain.UseCases.CreateForum;
using Moq;

namespace Forum.Domain.Tests.Authorization;

public class IntentionManagerShould
{
    [Fact]
    public void ReturnFalse_WhenNoMatchingResolver()
    {
        var sut = new IntentionManager(new IIntentionResolver[]
        {
            new Mock<IIntentionResolver<DomainErrorCode>>().Object,
            new Mock<IIntentionResolver<HttpStatusCode>>().Object
        }, new Mock<IIdentityProvider>().Object);

        sut.IsAllowed(ForumIntention.Create).Should().BeFalse();
    }

    [Theory]
    [InlineData(true, true)]
    [InlineData(false, false)]
    public void ReturnMatchingResolverResult(bool expectedResolverResult, bool expected)
    {
        var resolver = new Mock<IIntentionResolver<ForumIntention>>();
        resolver
            .Setup(r => r.IsAllowed(It.IsAny<IIdentity>(), It.IsAny<ForumIntention>()))
            .Returns(expectedResolverResult);

        var identityProvider = new Mock<IIdentityProvider>();
        identityProvider
            .Setup(p => p.Current)
            .Returns(new Identity(Guid.Parse("f2d6323a-6112-4c7b-9b3d-893c761ec265"), Guid.Empty));

        var sut = new IntentionManager(
            new IIntentionResolver[] { resolver.Object },
            identityProvider.Object);

        sut.IsAllowed(ForumIntention.Create).Should().Be(expected);
    }
}