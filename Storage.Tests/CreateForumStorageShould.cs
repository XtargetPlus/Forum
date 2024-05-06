using FluentAssertions;
using Forum.Domain.UseCases.CreateForum;
using Forum.Storage.Storages;
using Microsoft.EntityFrameworkCore;

namespace Forum.Storage.Tests;

public class CreateForumStorageShould : IClassFixture<StorageTextFixture>
{
    private readonly StorageTextFixture _fixture;
    private readonly CreateForumStorage _sut;

    public CreateForumStorageShould(StorageTextFixture fixture)
    {
        _fixture = fixture;
        _sut = new CreateForumStorage(_fixture.GetMapper(), _fixture.GetMemoryCache(), _fixture.GetDbContext());
    }

    [Fact]
    public async Task InsertNewForumToDatabase()
    {
        var forum = await _sut.CreateForum(new CreateForumCommand("Qwerty1"), CancellationToken.None);
        forum.ForumId.Should().NotBeEmpty();

        await using var dbContext = _fixture.GetDbContext();
        var forumTitles = await dbContext.Forums
            .Where(f => f.Id == forum.ForumId)
            .Select(f => f.Title)
            .ToArrayAsync();

        forumTitles.Should().HaveCount(1).And.Contain("Qwerty1");
    }
}