using System.Net.Http.Json;
using Domain.Dtos;
using FluentAssertions;

namespace E2E;

public class ForumEndpointsShould(ForumApiApplicationFactory factory) : IClassFixture<ForumApiApplicationFactory>
{
    // [Fact]
    public async Task CreateNewForum()
    {
        const string forumTitle = "6ba40980-d005-427f-bcfa-3b2c5e7bdd5b";

        using var httpClient = factory.CreateClient();

        using var getInitialForumsResponse = await httpClient.GetAsync("api/forums");
        getInitialForumsResponse.IsSuccessStatusCode.Should().BeTrue();
        var initialForums = await getInitialForumsResponse.Content.ReadFromJsonAsync<ForumDto[]>();
        initialForums
            .Should().NotBeNull().And
            .Subject.As<ForumDto[]>().Should().NotContain(f => f.Title.Equals(forumTitle));

        using var response = await httpClient.PostAsync("api/forums", 
            JsonContent.Create(new { Title = forumTitle }));

        response.IsSuccessStatusCode.Should().BeTrue();
        var forum = await response.Content.ReadFromJsonAsync<ForumDto>();
        forum
            .Should().NotBeNull().And
            .Subject.As<ForumDto>().Title.Should().Be(forumTitle);

        using var getForumsResponse = await httpClient.GetAsync("api/forums");
        var forums = await getForumsResponse.Content.ReadFromJsonAsync<ForumDto[]>();
        forums
            .Should().NotBeNullOrEmpty().And
            .Subject.As<ForumDto[]>().Should().Contain(f => f.ForumId.Equals(f.ForumId) && f.Title.Equals(forumTitle));
    }
}