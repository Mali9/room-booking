using RoomBooking.Core.Facilities.Shared;

namespace RoomBooking.Core.Facilities.Labs;

/// <summary>Prices Labs orders: multiplier, bulk discount, then VAT.</summary>
public sealed class LabsPricing(LabsPolicy policy, FacilityTax tax)
{
    public LabsQuote Quote(LabsItem item, int quantity)
    {
        var baseAmount = item.UnitPrice * quantity * policy.Multiplier;
        var discount = quantity >= policy.BulkThreshold
            ? FacilityMoney.Round(baseAmount * policy.BulkDiscountRate)
            : 0m;
        var handlingFee = 0m;
        var total = tax.Apply(baseAmount - discount + handlingFee);
        return new LabsQuote(FacilityMoney.Round(baseAmount), discount, handlingFee, total);
    }
}
