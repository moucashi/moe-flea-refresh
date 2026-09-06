using MoeFleaRefresh.Configuration;
using MoeFleaRefresh.Localization;
using SPTarkov.Common.Models.Logging;
using SPTarkov.DI.Annotations;
using SPTarkov.Server.Core.Extensions;
using SPTarkov.Server.Core.Generators.Ragfair;
using SPTarkov.Server.Core.Services.Ragfair;
using SPTarkov.Server.Core.Utils;

namespace MoeFleaRefresh.Services;

[Injectable(InjectionType = InjectionType.Singleton)]
public sealed class FleaRefreshService(
    ISptLogger<FleaRefreshService> logger,
    FleaRefreshConfigService configService,
    RagfairOfferHolder offerHolder,
    RagfairOfferGenerator offerGenerator,
    RagfairRequiredItemsService requiredItemsService,
    FleaRefreshLocalizer localizer)
{
    private readonly Lock refreshLock = new();

    public bool IsReady { get; set; }

    public void Refresh(FleaRefreshText reason)
    {
        if (!IsReady)
        {
            return;
        }

        using (refreshLock.EnterScope())
        {
            var offerIds = offerHolder.GetOffers()
                .Where(offer => offer.IsFakePlayerOffer())
                .Select(offer => offer.Id)
                .ToList();

            foreach (var offerId in offerIds)
            {
                offerHolder.RemoveOffer(offerId, checkTraderOffers: false);
            }

            offerHolder.ResetExpiredOfferIds();
            offerGenerator.GenerateDynamicOffers();
            requiredItemsService.InvalidateCache();
            logger.Success(localizer.Format(
                FleaRefreshText.RefreshCompleted,
                localizer.Text(reason),
                offerIds.Count));
        }
    }

    public bool RefreshAfterRaidEnabled => configService.Config.RefreshAfterRaid;
    public bool RefreshWhenFenceRefreshesEnabled => configService.Config.RefreshWhenFenceRefreshes;
}
