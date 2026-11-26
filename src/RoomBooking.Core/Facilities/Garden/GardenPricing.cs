using RoomBooking.Core.Facilities.Shared;
using RoomBooking.Core.Facilities.Kitchen;

namespace RoomBooking.Core.Facilities.Garden;

/// <summary>Prices Garden orders: multiplier, bulk discount, the Kitchen handling fee, then VAT.</summary>
public sealed class GardenPricing(GardenPolicy policy, FacilityTax tax, KitchenPolicy kitchenPolicy)
{
    public GardenQuote Quote(GardenItem item, int quantity)
    {
        var baseAmount = item.UnitPrice * quantity * policy.Multiplier;
        var discount = quantity >= policy.BulkThreshold
            ? FacilityMoney.Round(baseAmount * policy.BulkDiscountRate)
            : 0m;
        var handlingFee = kitchenPolicy.HandlingFee;
        var total = tax.Apply(baseAmount - discount + handlingFee);
        return new GardenQuote(FacilityMoney.Round(baseAmount), discount, handlingFee, total);
    }
}
