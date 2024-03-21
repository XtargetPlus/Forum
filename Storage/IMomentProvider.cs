namespace Storage;

internal interface IMomentProvider
{
    DateTimeOffset Now { get; }
}