using RoomBooking.Core.Facilities.Shared;

namespace RoomBooking.Core.Facilities.Studio;

/// <summary>Prices Studio orders: multiplier, bulk discount, then VAT.</summary>
public sealed class StudioPricing(StudioPolicy policy, FacilityTax tax)
{
    public StudioQuote Quote(StudioItem item, int quantity)
    {
        var baseAmount = item.UnitPrice * quantity * policy.Multiplier;
        var discount = quantity >= policy.BulkThreshold
            ? FacilityMoney.Round(baseAmount * policy.BulkDiscountRate)
            : 0m;
        var handlingFee = 0m;
        var total = tax.Apply(baseAmount - discount + handlingFee);
        return new StudioQuote(FacilityMoney.Round(baseAmount), discount, handlingFee, total);
    }
}
