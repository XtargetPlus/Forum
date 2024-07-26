namespace Forum.API.Models.Requests;

public class SignOnRequest
{
    public string Login { get; set; } = null!;
    public string Password { get; set; } = null!;
}
