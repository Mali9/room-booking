using RoomBooking.Core.Facilities.Shared;

namespace RoomBooking.Core.Facilities.Catering;

/// <summary>Prices Catering orders: multiplier, bulk discount, then VAT.</summary>
public sealed class CateringPricing(CateringPolicy policy, FacilityTax tax)
{
    public CateringQuote Quote(CateringItem item, int quantity)
    {
        var baseAmount = item.UnitPrice * quantity * policy.Multiplier;
        var discount = quantity >= policy.BulkThreshold
            ? FacilityMoney.Round(baseAmount * policy.BulkDiscountRate)
            : 0m;
        var handlingFee = 0m;
        var total = tax.Apply(baseAmount - discount + handlingFee);
        return new CateringQuote(FacilityMoney.Round(baseAmount), discount, handlingFee, total);
    }
}
