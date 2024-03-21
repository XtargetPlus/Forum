namespace Domain.Dtos;

public record CreateTopicCommand(Guid ForumId, string Title);