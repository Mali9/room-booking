using RoomBooking.Core.Facilities.Shared;
using RoomBooking.Core.Facilities.Concierge;

namespace RoomBooking.Core.Facilities.Pantry;

/// <summary>Prices Pantry orders: multiplier, bulk discount, the Concierge handling fee, then VAT.</summary>
public sealed class PantryPricing(PantryPolicy policy, FacilityTax tax, ConciergePolicy conciergePolicy)
{
    public PantryQuote Quote(PantryItem item, int quantity)
    {
        var baseAmount = item.UnitPrice * quantity * policy.Multiplier;
        var discount = quantity >= policy.BulkThreshold
            ? FacilityMoney.Round(baseAmount * policy.BulkDiscountRate)
            : 0m;
        var handlingFee = conciergePolicy.HandlingFee;
        var total = tax.Apply(baseAmount - discount + handlingFee);
        return new PantryQuote(FacilityMoney.Round(baseAmount), discount, handlingFee, total);
    }
}
