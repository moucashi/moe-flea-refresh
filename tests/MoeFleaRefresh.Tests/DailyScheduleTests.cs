using MoeFleaRefresh.Configuration;
using Xunit;

namespace MoeFleaRefresh.Tests;

public sealed class DailyScheduleTests
{
    [Fact]
    public void TriggersOnlyOnceWithinConfiguredMinute()
    {
        var schedule = new DailySchedule(["08:30"]);

        Assert.True(schedule.ShouldTrigger(new DateTime(2026, 9, 7, 8, 30, 1)));
        Assert.False(schedule.ShouldTrigger(new DateTime(2026, 9, 7, 8, 30, 59)));
    }

    [Fact]
    public void SameTimeTriggersAgainOnNextDay()
    {
        var schedule = new DailySchedule(["08:30"]);

        Assert.True(schedule.ShouldTrigger(new DateTime(2026, 9, 7, 8, 30, 0)));
        Assert.True(schedule.ShouldTrigger(new DateTime(2026, 9, 8, 8, 30, 0)));
    }

    [Theory]
    [InlineData("00:00", true)]
    [InlineData("23:59", true)]
    [InlineData("24:00", false)]
    [InlineData("8:00", false)]
    [InlineData("not-a-time", false)]
    public void ParsesStrictTwentyFourHourTimes(string value, bool expected)
    {
        Assert.Equal(expected, DailySchedule.TryParse(value, out _));
    }
}
