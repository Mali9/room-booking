using RoomBooking.Core.Facilities.Shared;

namespace RoomBooking.Core.Facilities.Kitchen;

/// <summary>Prices Kitchen orders: multiplier, bulk discount, then VAT.</summary>
public sealed class KitchenPricing(KitchenPolicy policy, FacilityTax tax)
{
    public KitchenQuote Quote(KitchenItem item, int quantity)
    {
        var baseAmount = item.UnitPrice * quantity * policy.Multiplier;
        var discount = quantity >= policy.BulkThreshold
            ? FacilityMoney.Round(baseAmount * policy.BulkDiscountRate)
            : 0m;
        var handlingFee = 0m;
        var total = tax.Apply(baseAmount - discount + handlingFee);
        return new KitchenQuote(FacilityMoney.Round(baseAmount), discount, handlingFee, total);
    }
}
