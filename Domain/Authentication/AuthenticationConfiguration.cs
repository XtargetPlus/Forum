namespace Forum.Domain.Authentication;

public class AuthenticationConfiguration
{
    public string Base64Key { get; set; } = null!;

    public byte[] Key => Convert.FromBase64String(Base64Key);
}