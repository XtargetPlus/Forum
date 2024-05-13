namespace Search.Domain.Dtos;

public class SearchResult
{
    public Guid EntityId { get; set; }
    public SearchEntityType EntityType { get; set; }
    public ICollection<string> Highlights { get; set; } = null!;
}