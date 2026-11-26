using RoomBooking.Core.Facilities.Shared;

namespace RoomBooking.Core.Facilities.Wellness;

/// <summary>Prices Wellness orders: multiplier, bulk discount, then VAT.</summary>
public sealed class WellnessPricing(WellnessPolicy policy, FacilityTax tax)
{
    public WellnessQuote Quote(WellnessItem item, int quantity)
    {
        var baseAmount = item.UnitPrice * quantity * policy.Multiplier;
        var discount = quantity >= policy.BulkThreshold
            ? FacilityMoney.Round(baseAmount * policy.BulkDiscountRate)
            : 0m;
        var handlingFee = 0m;
        var total = tax.Apply(baseAmount - discount + handlingFee);
        return new WellnessQuote(FacilityMoney.Round(baseAmount), discount, handlingFee, total);
    }
}
