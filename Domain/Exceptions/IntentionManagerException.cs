namespace Domain.Exceptions;

public class IntentionManagerException() : DomainException(DomainErrorCode.Forbidden403, "Action is not allowed");