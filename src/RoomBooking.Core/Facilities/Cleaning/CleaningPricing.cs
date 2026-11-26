using RoomBooking.Core.Facilities.Shared;

namespace RoomBooking.Core.Facilities.Cleaning;

/// <summary>Prices Cleaning orders: multiplier, bulk discount, then VAT.</summary>
public sealed class CleaningPricing(CleaningPolicy policy, FacilityTax tax)
{
    public CleaningQuote Quote(CleaningItem item, int quantity)
    {
        var baseAmount = item.UnitPrice * quantity * policy.Multiplier;
        var discount = quantity >= policy.BulkThreshold
            ? FacilityMoney.Round(baseAmount * policy.BulkDiscountRate)
            : 0m;
        var handlingFee = 0m;
        var total = tax.Apply(baseAmount - discount + handlingFee);
        return new CleaningQuote(FacilityMoney.Round(baseAmount), discount, handlingFee, total);
    }
}
