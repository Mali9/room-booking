using RoomBooking.Core.Facilities.Shared;
using RoomBooking.Core.Facilities.Security;

namespace RoomBooking.Core.Facilities.Lockers;

/// <summary>Prices Lockers orders: multiplier, bulk discount, the Security handling fee, then VAT.</summary>
public sealed class LockersPricing(LockersPolicy policy, FacilityTax tax, SecurityPolicy securityPolicy)
{
    public LockersQuote Quote(LockersItem item, int quantity)
    {
        var baseAmount = item.UnitPrice * quantity * policy.Multiplier;
        var discount = quantity >= policy.BulkThreshold
            ? FacilityMoney.Round(baseAmount * policy.BulkDiscountRate)
            : 0m;
        var handlingFee = securityPolicy.HandlingFee;
        var total = tax.Apply(baseAmount - discount + handlingFee);
        return new LockersQuote(FacilityMoney.Round(baseAmount), discount, handlingFee, total);
    }
}
