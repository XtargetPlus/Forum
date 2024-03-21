namespace Domain.Authentication;

internal class IdentityProvider : IIdentityProvider
{
    public IIdentity Current { get; set; } = null!;
}