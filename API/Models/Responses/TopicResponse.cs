namespace Forum.API.Models.Responses;

public class TopicResponse
{
    public Guid TopicId { get; set; }
    public string Title { get; set; } = null!;
    public DateTimeOffset CreatedAt { get; set; }
}
