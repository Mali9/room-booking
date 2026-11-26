using RoomBooking.Core.Facilities.Shared;

namespace RoomBooking.Core.Facilities.ItSupport;

/// <summary>Prices ItSupport orders: multiplier, bulk discount, then VAT.</summary>
public sealed class ItSupportPricing(ItSupportPolicy policy, FacilityTax tax)
{
    public ItSupportQuote Quote(ItSupportItem item, int quantity)
    {
        var baseAmount = item.UnitPrice * quantity * policy.Multiplier;
        var discount = quantity >= policy.BulkThreshold
            ? FacilityMoney.Round(baseAmount * policy.BulkDiscountRate)
            : 0m;
        var handlingFee = 0m;
        var total = tax.Apply(baseAmount - discount + handlingFee);
        return new ItSupportQuote(FacilityMoney.Round(baseAmount), discount, handlingFee, total);
    }
}
