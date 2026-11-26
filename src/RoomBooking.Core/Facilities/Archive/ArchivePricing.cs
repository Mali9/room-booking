using RoomBooking.Core.Facilities.Shared;
using RoomBooking.Core.Facilities.Badges;

namespace RoomBooking.Core.Facilities.Archive;

/// <summary>Prices Archive orders: multiplier, bulk discount, the Badges handling fee, then VAT.</summary>
public sealed class ArchivePricing(ArchivePolicy policy, FacilityTax tax, BadgesPolicy badgesPolicy)
{
    public ArchiveQuote Quote(ArchiveItem item, int quantity)
    {
        var baseAmount = item.UnitPrice * quantity * policy.Multiplier;
        var discount = quantity >= policy.BulkThreshold
            ? FacilityMoney.Round(baseAmount * policy.BulkDiscountRate)
            : 0m;
        var handlingFee = badgesPolicy.HandlingFee;
        var total = tax.Apply(baseAmount - discount + handlingFee);
        return new ArchiveQuote(FacilityMoney.Round(baseAmount), discount, handlingFee, total);
    }
}
