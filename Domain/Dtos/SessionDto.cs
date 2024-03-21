namespace Domain.Dtos;

public class SessionDto
{
    public Guid UserId { get; set; }
    public DateTimeOffset ExpiresAt { get; set; }
}