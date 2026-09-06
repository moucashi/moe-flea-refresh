using System.Text.Json;
using MoeFleaRefresh.Configuration;
using Xunit;

namespace MoeFleaRefresh.Tests;

public sealed class FleaRefreshConfigTests
{
    [Fact]
    public void DeserializesCamelCaseConfigurationWithCaseSensitiveOptions()
    {
        const string json = """
            {
              "refreshAfterRaid": true,
              "refreshWhenFenceRefreshes": true,
              "scheduledTimes": {
                "enabled": true,
                "times": ["08:00", "20:30"]
              },
              "fixedInterval": {
                "enabled": true,
                "minutes": 45.5
              }
            }
            """;

        var config = JsonSerializer.Deserialize<FleaRefreshConfig>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = false
        });

        Assert.NotNull(config);
        Assert.True(config.RefreshAfterRaid);
        Assert.True(config.RefreshWhenFenceRefreshes);
        Assert.True(config.ScheduledTimes.Enabled);
        Assert.Equal(["08:00", "20:30"], config.ScheduledTimes.Times);
        Assert.True(config.FixedInterval.Enabled);
        Assert.Equal(45.5, config.FixedInterval.Minutes);
    }
}
