using System.Net.Http.Json;
using FluentAssertions;
using Forum.Domain.Authentication;

namespace Forum.E2E;

public class AccountEndpointsShould(ForumApiApplicationFactory factory) : IClassFixture<ForumApiApplicationFactory>
{
    [Fact]
    public async Task SignInAfterSignOn()
    {
        using var httpClient = factory.CreateClient();

        using var signOnResponse = await httpClient.PostAsync("api/accounts", JsonContent.Create(new { Login = "Test", Password = "qwerty" }));
        signOnResponse.IsSuccessStatusCode.Should().BeTrue();
        var createdUser = await signOnResponse.Content.ReadFromJsonAsync<Identity>();
        createdUser.Should().NotBeNull();

        using var signInResponse = await httpClient.PostAsync("api/accounts/signin", JsonContent.Create(new { Login = "Test", Password = "qwerty" }));
        signInResponse.IsSuccessStatusCode.Should().BeTrue();

        var signedInUser = await signInResponse.Content.ReadFromJsonAsync<Identity>();
        signedInUser.Should().NotBeNull()
            .And.Subject.As<Identity>().UserId.Should().Be(createdUser!.UserId);

        var createForumResponse = await httpClient.PostAsync("api/forums", JsonContent.Create(new { Title = "test" }));
        createForumResponse.IsSuccessStatusCode.Should().BeTrue();
    }
}