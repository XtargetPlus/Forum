using FluentAssertions;
using Forum.Domain.Dtos;
using Forum.Storage.Entities;
using Forum.Storage.Storages;

namespace Forum.Storage.Tests;

public class CreateCommentStorageShould(StorageTextFixture fixture) : IClassFixture<StorageTextFixture>
{
    private readonly CreateCommentStorage _sut = new(fixture.GetMapper(), fixture.GetDbContext(), TimeProvider.System);

    [Fact]
    public async Task ReturnNullForTop_WhenNoMatchingTopicInDb()
    {
        var topicId = Guid.Parse("7618547a-1fba-4e3c-acf6-a48e2c4ecda5");

        var actual = await _sut.FindTopic(topicId, CancellationToken.None);
        actual.Should().BeNull();
    }

    [Fact]
    public async Task ReturnFoundTopic_WhenTopicIsPresentInDb()
    {
        var topicId = Guid.Parse("b4439bdc-1844-48d2-bf26-2c2a3b1ce460");
        var userId = Guid.Parse("815822cf-d3b8-42ff-9f3c-3049a2e942d4");
        var forumId = Guid.Parse("e77b5bd6-196a-4314-ba3d-8ea3ff33b4b7");
        var now = new DateTimeOffset(1999, 9, 15, 10, 10, 10, TimeSpan.Zero);

        var db = fixture.GetDbContext();
        await db.Topics.AddAsync(new Topic
        {
            Id = topicId,
            Title = "qwerty",
            Forum = new Entities.Forum
            {
                Id = forumId,
                Title = "qwerty"
            },
            Author = new User
            {
                Id = userId,
                Login = "qwerty",
                PasswordHash = "qwerty"u8.ToArray(),
                Salt = "qwerty"u8.ToArray()
            },
            CreatedAt = now
        });
        await db.SaveChangesAsync();

        var actual = await _sut.FindTopic(topicId, CancellationToken.None);
        actual.Should().NotBeNull().And
            .Subject.As<TopicDto>().Should().BeEquivalentTo(new TopicDto
            {
                TopicId = topicId,
                Title = "qwerty",
                CreatedAt = now,
                ForumId = forumId,
                UserId = userId
            });
    }

    [Fact]
    public async Task ReturnNewlyCreatedComment_WhenCreating()
    {
        var topicId = Guid.Parse("a46c10ae-f2c3-45f2-b817-358d1a456a9b");
        var userId = Guid.Parse("a42a2931-c07f-4368-bd45-cefddb7986ae");

        var db = fixture.GetDbContext();
        await db.Topics.AddAsync(new Topic
        {
            Id = topicId,
            Title = "qwerty",
            Forum = new Entities.Forum
            {
                Title = "qwerty"
            },
            Author = new User
            {
                Id = userId,
                Login = "qwerty",
                PasswordHash = "qwerty"u8.ToArray(),
                Salt = "qwerty"u8.ToArray()
            },
            CreatedAt = TimeProvider.System.GetUtcNow()
        });
        await db.SaveChangesAsync();

        var actual = await _sut.Create(topicId, userId, "qwerty", CancellationToken.None);
        actual.Should().NotBeNull().And
            .Subject.As<CommentDto>().Should().BeEquivalentTo(
                new CommentDto
                {
                    Text = "qwerty",
                    AuthorLogin = "qwerty"
                },
                cfg => cfg.Excluding(c => c.CommentId).Excluding(c => c.CreatedAt));
    }
}