using System.Collections.Concurrent;
using System.Diagnostics;
using System.Diagnostics.Metrics;

namespace Forum.Domain.Monitoring;

public class DomainMetrics(IMeterFactory meterFactory)
{
    internal static readonly ActivitySource ActivitySource = new("Domain");

    private readonly Meter _meter = meterFactory.Create("Domain");
    private readonly ConcurrentDictionary<string, Counter<int>> _counters = new();

    public void IncrementCount(string name, int value, IDictionary<string, object?>? additionalTags = null)
    {
        var counter = _counters.GetOrAdd(name, _ => _meter.CreateCounter<int>(name));
        counter.Add(value, additionalTags?.ToArray() ?? ReadOnlySpan<KeyValuePair<string, object?>>.Empty);
    }

    public static IDictionary<string, object?> ResultTags(bool success) => new Dictionary<string, object?>
    {
        ["success"] = success
    };
}