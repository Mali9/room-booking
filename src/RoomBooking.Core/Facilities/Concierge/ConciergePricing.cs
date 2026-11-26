using RoomBooking.Core.Facilities.Shared;

namespace RoomBooking.Core.Facilities.Concierge;

/// <summary>Prices Concierge orders: multiplier, bulk discount, then VAT.</summary>
public sealed class ConciergePricing(ConciergePolicy policy, FacilityTax tax)
{
    public ConciergeQuote Quote(ConciergeItem item, int quantity)
    {
        var baseAmount = item.UnitPrice * quantity * policy.Multiplier;
        var discount = quantity >= policy.BulkThreshold
            ? FacilityMoney.Round(baseAmount * policy.BulkDiscountRate)
            : 0m;
        var handlingFee = 0m;
        var total = tax.Apply(baseAmount - discount + handlingFee);
        return new ConciergeQuote(FacilityMoney.Round(baseAmount), discount, handlingFee, total);
    }
}
