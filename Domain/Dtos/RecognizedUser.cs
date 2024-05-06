namespace Forum.Domain.Dtos;

public class RecognizedUser
{
    public Guid UserId { get; set; }
    public byte[] Salt { get; set; } = null!;
    public byte[] PasswordHash { get; set; } = null!;
}