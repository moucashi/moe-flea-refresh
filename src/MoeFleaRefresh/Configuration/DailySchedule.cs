using System.Globalization;

namespace MoeFleaRefresh.Configuration;

public sealed class DailySchedule(IEnumerable<string> configuredTimes)
{
    private readonly HashSet<TimeOnly> times = configuredTimes
        .Select(value => TryParse(value, out var time) ? time : (TimeOnly?)null)
        .Where(time => time.HasValue)
        .Select(time => time!.Value)
        .ToHashSet();

    private DateOnly? lastTriggeredDate;
    private TimeOnly? lastTriggeredTime;

    public bool ShouldTrigger(DateTime now)
    {
        var minute = new TimeOnly(now.Hour, now.Minute);
        var date = DateOnly.FromDateTime(now);
        if (!times.Contains(minute) || (lastTriggeredDate == date && lastTriggeredTime == minute))
        {
            return false;
        }

        lastTriggeredDate = date;
        lastTriggeredTime = minute;
        return true;
    }

    public static bool TryParse(string? value, out TimeOnly time)
    {
        return TimeOnly.TryParseExact(value, "HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out time);
    }
}

