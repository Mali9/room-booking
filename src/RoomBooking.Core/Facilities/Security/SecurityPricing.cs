using RoomBooking.Core.Facilities.Shared;

namespace RoomBooking.Core.Facilities.Security;

/// <summary>Prices Security orders: multiplier, bulk discount, then VAT.</summary>
public sealed class SecurityPricing(SecurityPolicy policy, FacilityTax tax)
{
    public SecurityQuote Quote(SecurityItem item, int quantity)
    {
        var baseAmount = item.UnitPrice * quantity * policy.Multiplier;
        var discount = quantity >= policy.BulkThreshold
            ? FacilityMoney.Round(baseAmount * policy.BulkDiscountRate)
            : 0m;
        var handlingFee = 0m;
        var total = tax.Apply(baseAmount - discount + handlingFee);
        return new SecurityQuote(FacilityMoney.Round(baseAmount), discount, handlingFee, total);
    }
}
