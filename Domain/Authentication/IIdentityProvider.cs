namespace Forum.Domain.Authentication;

public interface IIdentityProvider
{
    IIdentity Current { get; set; }
}