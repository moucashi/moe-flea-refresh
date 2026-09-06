using System.Reflection;
using MoeFleaRefresh.Services;
using MoeFleaRefresh.Localization;
using SPTarkov.Reflection.Patching;
using SPTarkov.Server.Core.Controllers;
using SPTarkov.Server.Core.Models.Eft.Match;
using SPTarkov.Server.Core.Models.Enums;

namespace MoeFleaRefresh.Patches;

public sealed class EndLocalRaidPatch : AbstractPatch
{
    private static FleaRefreshService service = null!;

    public EndLocalRaidPatch(FleaRefreshService fleaRefreshService)
    {
        service = fleaRefreshService;
    }

    protected override MethodBase GetTargetMethod() =>
        typeof(MatchController).GetMethod(nameof(MatchController.EndLocalRaidAsync))!;

    [PatchPostfix]
    public static void Postfix(EndLocalRaidRequestData __1, ref Task __result)
    {
        if (!service.RefreshAfterRaidEnabled || __1.Results?.Result is null or ExitStatus.TRANSIT)
        {
            return;
        }

        __result = RefreshAfterSuccessfulRaidEnd(__result);
    }

    private static async Task RefreshAfterSuccessfulRaidEnd(Task original)
    {
        await original.ConfigureAwait(false);
        service.Refresh(FleaRefreshText.ReasonRaidEnded);
    }
}
