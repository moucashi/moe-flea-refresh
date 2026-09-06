namespace MoeFleaRefresh.Configuration;

public sealed class FleaRefreshConfig
{
    public bool RefreshAfterRaid { get; init; }
    public bool RefreshWhenFenceRefreshes { get; init; }
    public ScheduledTimesConfig ScheduledTimes { get; init; } = new();
    public FixedIntervalConfig FixedInterval { get; init; } = new();
}

public sealed class ScheduledTimesConfig
{
    public bool Enabled { get; init; }
    public List<string> Times { get; init; } = [];
}

public sealed class FixedIntervalConfig
{
    public bool Enabled { get; init; }
    public double Minutes { get; init; } = 60;
}

