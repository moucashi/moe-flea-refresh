using System.Globalization;
using SPTarkov.DI.Annotations;
using SPTarkov.Server.Core.Services.Locales;

namespace MoeFleaRefresh.Localization;

[Injectable(InjectionType = InjectionType.Singleton)]
public sealed class FleaRefreshLocalizer(LocaleService localeService)
{
    private static readonly IReadOnlyDictionary<FleaRefreshText, string> English =
        new Dictionary<FleaRefreshText, string>
        {
            [FleaRefreshText.ModLoaded] = "[Moe Flea Refresh] Mod loaded",
            [FleaRefreshText.ConfigLoaded] = "[Moe Flea Refresh] Configuration loaded",
            [FleaRefreshText.ConfigLoadFailed] = "[Moe Flea Refresh] Failed to load configuration; all triggers have been disabled: {0}",
            [FleaRefreshText.InvalidInterval] = "fixedInterval.minutes must be greater than 0",
            [FleaRefreshText.InvalidScheduledTime] = "Invalid time in scheduledTimes.times: {0} (expected HH:mm)",
            [FleaRefreshText.RefreshCompleted] = "[Moe Flea Refresh] Flea market refreshed ({0}); replaced {1} AI offers",
            [FleaRefreshText.ReasonRaidEnded] = "raid ended",
            [FleaRefreshText.ReasonFenceRefreshed] = "Fence refreshed",
            [FleaRefreshText.ReasonScheduledTime] = "scheduled time",
            [FleaRefreshText.ReasonFixedInterval] = "fixed interval",
        };

    private static readonly IReadOnlyDictionary<FleaRefreshText, string> Chinese =
        new Dictionary<FleaRefreshText, string>
        {
            [FleaRefreshText.ModLoaded] = "[Moe Flea Refresh] 模组已加载",
            [FleaRefreshText.ConfigLoaded] = "[Moe Flea Refresh] 配置已加载",
            [FleaRefreshText.ConfigLoadFailed] = "[Moe Flea Refresh] 配置加载失败，已禁用全部触发器: {0}",
            [FleaRefreshText.InvalidInterval] = "fixedInterval.minutes 必须大于 0",
            [FleaRefreshText.InvalidScheduledTime] = "scheduledTimes.times 中的时间无效: {0}（应为 HH:mm）",
            [FleaRefreshText.RefreshCompleted] = "[Moe Flea Refresh] 已刷新跳蚤市场（{0}），替换 {1} 条 AI 报价",
            [FleaRefreshText.ReasonRaidEnded] = "战局结束",
            [FleaRefreshText.ReasonFenceRefreshed] = "黑商刷新",
            [FleaRefreshText.ReasonScheduledTime] = "指定时间点",
            [FleaRefreshText.ReasonFixedInterval] = "固定时间间隔",
        };

    public string Text(FleaRefreshText key)
    {
        var table = IsChinese(SafeGetServerLocale()) ? Chinese : English;
        return table.TryGetValue(key, out var value) ? value : English[key];
    }

    public string Format(FleaRefreshText key, params object?[] args)
    {
        return string.Format(CultureInfo.InvariantCulture, Text(key), args);
    }

    public static bool IsChinese(string? languageCode)
    {
        if (string.IsNullOrWhiteSpace(languageCode))
        {
            return false;
        }

        var normalized = languageCode.Trim().Replace('_', '-').ToLowerInvariant();
        return normalized is "ch" or "chs" or "cht" or "cn" or "zh" or "zh-cn" or "zh-hans" or "zh-hant" or "zh-tw"
            || normalized.StartsWith("zh-", StringComparison.Ordinal)
            || normalized.StartsWith("ch-", StringComparison.Ordinal)
            || normalized.StartsWith("cn-", StringComparison.Ordinal);
    }

    private string? SafeGetServerLocale()
    {
        try
        {
            return localeService.GetDesiredServerLocale();
        }
        catch
        {
            return null;
        }
    }
}

