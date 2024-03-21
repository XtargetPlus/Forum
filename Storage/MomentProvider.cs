namespace Storage;

internal class MomentProvider : IMomentProvider
{
    public DateTimeOffset Now { get; } = DateTimeOffset.UtcNow;
}