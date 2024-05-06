namespace Forum.Domain.Dtos;

public class TopicDto
{
    public Guid TopicId { get; set; }
    public Guid UserId { get; set; }
    public Guid ForumId { get; set; }
    public string Title { get; set; } = null!;
    public DateTimeOffset CreatedAt { get; set; }
}