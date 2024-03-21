namespace Domain.Exceptions;

public class ForumNotFoundException(Guid forumId) : DomainException(DomainErrorCode.Gone410, $"Forum with Id {forumId} was not found");