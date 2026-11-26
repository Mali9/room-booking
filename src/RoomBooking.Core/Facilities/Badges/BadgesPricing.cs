using RoomBooking.Core.Facilities.Shared;

namespace RoomBooking.Core.Facilities.Badges;

/// <summary>Prices Badges orders: multiplier, bulk discount, then VAT.</summary>
public sealed class BadgesPricing(BadgesPolicy policy, FacilityTax tax)
{
    public BadgesQuote Quote(BadgesItem item, int quantity)
    {
        var baseAmount = item.UnitPrice * quantity * policy.Multiplier;
        var discount = quantity >= policy.BulkThreshold
            ? FacilityMoney.Round(baseAmount * policy.BulkDiscountRate)
            : 0m;
        var handlingFee = 0m;
        var total = tax.Apply(baseAmount - discount + handlingFee);
        return new BadgesQuote(FacilityMoney.Round(baseAmount), discount, handlingFee, total);
    }
}
