namespace Forum.Domain.Authentication;

public class Identity(Guid userId, Guid sessionId) : IIdentity
{
    public static Identity Guest = new(Guid.Empty, Guid.Empty);

    public Guid UserId { get; } = userId;
    public Guid SessionId { get; } = sessionId;
}