namespace Search.Domain.Dtos;

public class SearchEntityDto
{
    public Guid EntityId { get; set; }
    public SearchEntityType EntityType { get; set; }
    public string? Title { get; set; }
    public string? Text { get; set; }
}

public enum SearchEntityType
{
    ForumTopic,
    ForumComment
}