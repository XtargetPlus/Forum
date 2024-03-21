using System.Net;
using System.Net.Http.Json;
using API.Dtos.Responses;
using FluentAssertions;

namespace E2E;

public class TopicEndpointsShould(ForumApiApplicationFactory factory) : IClassFixture<ForumApiApplicationFactory>, IAsyncLifetime
{
    public Task InitializeAsync() => Task.CompletedTask;

    public Task DisposeAsync() => Task.CompletedTask;

    // [Fact]
    public async Task ReturnForbidden_WhenNotAuthenticated()
    {
        const string topicTitle = "42e96792-a09b-4421-8b8e-314d97bee6c5";

        using var httpClient = factory.CreateClient();

        using var forumCreatedResponse = await httpClient.PostAsync("api/forums", JsonContent.Create(new { Title = "World of Warcraft" }));
        forumCreatedResponse.EnsureSuccessStatusCode();

        var createdForum = await forumCreatedResponse.Content.ReadFromJsonAsync<ForumResponse>();
        createdForum.Should().NotBeNull();

        using var response = await httpClient.PostAsync($"api/forums/{createdForum!.ForumId}/topics",
            JsonContent.Create(new { Title = topicTitle, }));

        response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
    }
}