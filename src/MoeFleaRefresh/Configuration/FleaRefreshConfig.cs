using System.Text.Json.Serialization;

namespace MoeFleaRefresh.Configuration;

public sealed class FleaRefreshConfig
{
    [JsonPropertyName("refreshAfterRaid")]
    public bool RefreshAfterRaid { get; init; }

    [JsonPropertyName("refreshWhenFenceRefreshes")]
    public bool RefreshWhenFenceRefreshes { get; init; }

    [JsonPropertyName("scheduledTimes")]
    public ScheduledTimesConfig ScheduledTimes { get; init; } = new();

    [JsonPropertyName("fixedInterval")]
    public FixedIntervalConfig FixedInterval { get; init; } = new();
}

public sealed class ScheduledTimesConfig
{
    [JsonPropertyName("enabled")]
    public bool Enabled { get; init; }

    [JsonPropertyName("times")]
    public List<string> Times { get; init; } = [];
}

public sealed class FixedIntervalConfig
{
    [JsonPropertyName("enabled")]
    public bool Enabled { get; init; }

    [JsonPropertyName("minutes")]
    public double Minutes { get; init; } = 60;
}
