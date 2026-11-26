using RoomBooking.Core.Facilities.Shared;

namespace RoomBooking.Core.Facilities.Keys;

/// <summary>Prices Keys orders: multiplier, bulk discount, then VAT.</summary>
public sealed class KeysPricing(KeysPolicy policy, FacilityTax tax)
{
    public KeysQuote Quote(KeysItem item, int quantity)
    {
        var baseAmount = item.UnitPrice * quantity * policy.Multiplier;
        var discount = quantity >= policy.BulkThreshold
            ? FacilityMoney.Round(baseAmount * policy.BulkDiscountRate)
            : 0m;
        var handlingFee = 0m;
        var total = tax.Apply(baseAmount - discount + handlingFee);
        return new KeysQuote(FacilityMoney.Round(baseAmount), discount, handlingFee, total);
    }
}
