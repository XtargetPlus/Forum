namespace Forum.API.Models.Responses;

public class CommentResponse
{
    public Guid CommentId { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public string Text { get; set; } = null!;
    public string AuthorLogin { get; set; } = null!;
}