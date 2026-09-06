using MoeFleaRefresh.Configuration;
using MoeFleaRefresh.Patches;
using MoeFleaRefresh.Services;
using MoeFleaRefresh.Localization;
using SPTarkov.Common.Models.Logging;
using SPTarkov.DI.Annotations;
using SPTarkov.Server.Core.DI;

namespace MoeFleaRefresh;

[Injectable(InjectionType = InjectionType.Singleton, TypePriority = OnLoadOrder.PostLoad + 1)]
public sealed class MoeFleaRefreshMod(
    ISptLogger<MoeFleaRefreshMod> logger,
    FleaRefreshConfigService configService,
    FleaRefreshService refreshService,
    FleaRefreshLocalizer localizer) : IOnLoad, IOnUpdate
{
    private DailySchedule dailySchedule = new([]);
    private DateTime nextIntervalRefreshUtc = DateTime.MaxValue;

    public Task OnLoadAsync(CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        configService.Load();
        dailySchedule = new DailySchedule(configService.Config.ScheduledTimes.Times);
        ResetInterval(DateTime.UtcNow);

        new EndLocalRaidPatch(refreshService).Enable();
        new FenceRefreshPatch(refreshService).Enable();
        refreshService.IsReady = true;
        logger.Success(localizer.Text(FleaRefreshText.ModLoaded));
        return Task.CompletedTask;
    }

    public Task<bool> OnUpdateAsync(long secondsSinceLastRun, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var nowUtc = DateTime.UtcNow;
        var config = configService.Config;

        if (config.ScheduledTimes.Enabled && dailySchedule.ShouldTrigger(DateTime.Now))
        {
            refreshService.Refresh(FleaRefreshText.ReasonScheduledTime);
        }

        if (config.FixedInterval.Enabled && nowUtc >= nextIntervalRefreshUtc)
        {
            refreshService.Refresh(FleaRefreshText.ReasonFixedInterval);
            ResetInterval(nowUtc);
        }

        return Task.FromResult(true);
    }

    private void ResetInterval(DateTime nowUtc)
    {
        nextIntervalRefreshUtc = configService.Config.FixedInterval.Enabled
            ? nowUtc.AddMinutes(configService.Config.FixedInterval.Minutes)
            : DateTime.MaxValue;
    }
}
