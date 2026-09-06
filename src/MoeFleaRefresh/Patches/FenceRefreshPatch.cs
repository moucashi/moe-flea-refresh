using System.Reflection;
using MoeFleaRefresh.Services;
using MoeFleaRefresh.Localization;
using SPTarkov.Reflection.Patching;
using SPTarkov.Server.Core.Services.Commerce;

namespace MoeFleaRefresh.Patches;

public sealed class FenceRefreshPatch : AbstractPatch
{
    private static FleaRefreshService service = null!;

    public FenceRefreshPatch(FleaRefreshService fleaRefreshService)
    {
        service = fleaRefreshService;
    }

    protected override MethodBase GetTargetMethod() =>
        typeof(FenceService).GetMethod(nameof(FenceService.GenerateFenceAssorts))!;

    [PatchPostfix]
    public static void Postfix()
    {
        if (service.RefreshWhenFenceRefreshesEnabled)
        {
            service.Refresh(FleaRefreshText.ReasonFenceRefreshed);
        }
    }
}
