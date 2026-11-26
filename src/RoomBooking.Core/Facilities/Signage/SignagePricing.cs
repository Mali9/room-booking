using RoomBooking.Core.Facilities.Shared;

namespace RoomBooking.Core.Facilities.Signage;

/// <summary>Prices Signage orders: multiplier, bulk discount, then VAT.</summary>
public sealed class SignagePricing(SignagePolicy policy, FacilityTax tax)
{
    public SignageQuote Quote(SignageItem item, int quantity)
    {
        var baseAmount = item.UnitPrice * quantity * policy.Multiplier;
        var discount = quantity >= policy.BulkThreshold
            ? FacilityMoney.Round(baseAmount * policy.BulkDiscountRate)
            : 0m;
        var handlingFee = 0m;
        var total = tax.Apply(baseAmount - discount + handlingFee);
        return new SignageQuote(FacilityMoney.Round(baseAmount), discount, handlingFee, total);
    }
}
