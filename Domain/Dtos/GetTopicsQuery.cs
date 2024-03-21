namespace Domain.Dtos;

public record GetTopicsQuery(Guid ForumId, int Skip, int Take);