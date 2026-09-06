using System.Reflection;
using MoeFleaRefresh.Localization;
using SPTarkov.Common.Models.Logging;
using SPTarkov.DI.Annotations;
using SPTarkov.Server.Core.Helpers.Server;

namespace MoeFleaRefresh.Configuration;

[Injectable(InjectionType = InjectionType.Singleton)]
public sealed class FleaRefreshConfigService(
    ISptLogger<FleaRefreshConfigService> logger,
    ModHelper modHelper,
    FleaRefreshLocalizer localizer)
{
    public FleaRefreshConfig Config { get; private set; } = new();

    public void Load()
    {
        try
        {
            var modPath = modHelper.GetAbsolutePathToModFolder(Assembly.GetExecutingAssembly());
            Config = modHelper.GetJsonDataFromFile<FleaRefreshConfig>(modPath, "config.json") ?? new FleaRefreshConfig();
            Validate();
            logger.Success(localizer.Text(FleaRefreshText.ConfigLoaded));
        }
        catch (Exception exception)
        {
            Config = new FleaRefreshConfig();
            logger.Error(localizer.Format(FleaRefreshText.ConfigLoadFailed, exception.Message));
        }
    }

    private void Validate()
    {
        if (Config.FixedInterval.Enabled && Config.FixedInterval.Minutes <= 0)
        {
            throw new InvalidDataException(localizer.Text(FleaRefreshText.InvalidInterval));
        }

        foreach (var value in Config.ScheduledTimes.Times)
        {
            if (!DailySchedule.TryParse(value, out _))
            {
                throw new InvalidDataException(localizer.Format(FleaRefreshText.InvalidScheduledTime, value));
            }
        }
    }
}
