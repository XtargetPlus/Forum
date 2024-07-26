namespace Forum.Domain.Exceptions;

public class TopicNotFoundException(Guid topicId) : DomainException(DomainErrorCode.Gone410, $"Topic with id {topicId} was not found");