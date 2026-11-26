using RoomBooking.Core.Facilities.Shared;

namespace RoomBooking.Core.Facilities.Uniforms;

/// <summary>Prices Uniforms orders: multiplier, bulk discount, then VAT.</summary>
public sealed class UniformsPricing(UniformsPolicy policy, FacilityTax tax)
{
    public UniformsQuote Quote(UniformsItem item, int quantity)
    {
        var baseAmount = item.UnitPrice * quantity * policy.Multiplier;
        var discount = quantity >= policy.BulkThreshold
            ? FacilityMoney.Round(baseAmount * policy.BulkDiscountRate)
            : 0m;
        var handlingFee = 0m;
        var total = tax.Apply(baseAmount - discount + handlingFee);
        return new UniformsQuote(FacilityMoney.Round(baseAmount), discount, handlingFee, total);
    }
}
