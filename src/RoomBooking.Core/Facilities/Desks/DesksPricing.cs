using RoomBooking.Core.Facilities.Shared;

namespace RoomBooking.Core.Facilities.Desks;

/// <summary>Prices Desks orders: multiplier, bulk discount, then VAT.</summary>
public sealed class DesksPricing(DesksPolicy policy, FacilityTax tax)
{
    public DesksQuote Quote(DesksItem item, int quantity)
    {
        var baseAmount = item.UnitPrice * quantity * policy.Multiplier;
        var discount = quantity >= policy.BulkThreshold
            ? FacilityMoney.Round(baseAmount * policy.BulkDiscountRate)
            : 0m;
        var handlingFee = 0m;
        var total = tax.Apply(baseAmount - discount + handlingFee);
        return new DesksQuote(FacilityMoney.Round(baseAmount), discount, handlingFee, total);
    }
}
