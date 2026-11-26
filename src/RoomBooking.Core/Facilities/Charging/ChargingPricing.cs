using RoomBooking.Core.Facilities.Shared;

namespace RoomBooking.Core.Facilities.Charging;

/// <summary>Prices Charging orders: multiplier, bulk discount, then VAT.</summary>
public sealed class ChargingPricing(ChargingPolicy policy, FacilityTax tax)
{
    public ChargingQuote Quote(ChargingItem item, int quantity)
    {
        var baseAmount = item.UnitPrice * quantity * policy.Multiplier;
        var discount = quantity >= policy.BulkThreshold
            ? FacilityMoney.Round(baseAmount * policy.BulkDiscountRate)
            : 0m;
        var handlingFee = 0m;
        var total = tax.Apply(baseAmount - discount + handlingFee);
        return new ChargingQuote(FacilityMoney.Round(baseAmount), discount, handlingFee, total);
    }
}
