using RoomBooking.Core.Facilities.Shared;

namespace RoomBooking.Core.Facilities.Lounge;

/// <summary>Prices Lounge orders: multiplier, bulk discount, then VAT.</summary>
public sealed class LoungePricing(LoungePolicy policy, FacilityTax tax)
{
    public LoungeQuote Quote(LoungeItem item, int quantity)
    {
        var baseAmount = item.UnitPrice * quantity * policy.Multiplier;
        var discount = quantity >= policy.BulkThreshold
            ? FacilityMoney.Round(baseAmount * policy.BulkDiscountRate)
            : 0m;
        var handlingFee = 0m;
        var total = tax.Apply(baseAmount - discount + handlingFee);
        return new LoungeQuote(FacilityMoney.Round(baseAmount), discount, handlingFee, total);
    }
}
