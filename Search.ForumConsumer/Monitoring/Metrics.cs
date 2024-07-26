using System.Diagnostics;

namespace Search.ForumConsumer.Monitoring;

public class Metrics
{
    public const string ApplicationName = "Search.ForumConsumer";
    internal static readonly ActivitySource ActivitySource = new(ApplicationName);
}