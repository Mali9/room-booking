using RoomBooking.Core.Facilities.Shared;

namespace RoomBooking.Core.Facilities.Furniture;

/// <summary>Prices Furniture orders: multiplier, bulk discount, then VAT.</summary>
public sealed class FurniturePricing(FurniturePolicy policy, FacilityTax tax)
{
    public FurnitureQuote Quote(FurnitureItem item, int quantity)
    {
        var baseAmount = item.UnitPrice * quantity * policy.Multiplier;
        var discount = quantity >= policy.BulkThreshold
            ? FacilityMoney.Round(baseAmount * policy.BulkDiscountRate)
            : 0m;
        var handlingFee = 0m;
        var total = tax.Apply(baseAmount - discount + handlingFee);
        return new FurnitureQuote(FacilityMoney.Round(baseAmount), discount, handlingFee, total);
    }
}
