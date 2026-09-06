using System.Reflection;
using SPTarkov.Common.Models.Logging;
using SPTarkov.DI.Annotations;
using SPTarkov.Server.Core.Helpers.Server;

namespace MoeFleaRefresh.Configuration;

[Injectable(InjectionType = InjectionType.Singleton)]
public sealed class FleaRefreshConfigService(
    ISptLogger<FleaRefreshConfigService> logger,
    ModHelper modHelper)
{
    public FleaRefreshConfig Config { get; private set; } = new();

    public void Load()
    {
        try
        {
            var modPath = modHelper.GetAbsolutePathToModFolder(Assembly.GetExecutingAssembly());
            Config = modHelper.GetJsonDataFromFile<FleaRefreshConfig>(modPath, "config.json") ?? new FleaRefreshConfig();
            Validate();
            logger.Success("[Moe Flea Refresh] 配置已加载");
        }
        catch (Exception exception)
        {
            Config = new FleaRefreshConfig();
            logger.Error($"[Moe Flea Refresh] 配置加载失败，已禁用全部触发器: {exception.Message}");
        }
    }

    private void Validate()
    {
        if (Config.FixedInterval.Enabled && Config.FixedInterval.Minutes <= 0)
        {
            throw new InvalidDataException("fixedInterval.minutes 必须大于 0");
        }

        foreach (var value in Config.ScheduledTimes.Times)
        {
            if (!DailySchedule.TryParse(value, out _))
            {
                throw new InvalidDataException($"scheduledTimes.times 中的时间无效: {value}（应为 HH:mm）");
            }
        }
    }
}

