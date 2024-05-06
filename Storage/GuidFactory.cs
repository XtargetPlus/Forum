namespace Forum.Storage;

internal class GuidFactory : IGuidFactory
{
    public Guid Create() => Guid.NewGuid();
}