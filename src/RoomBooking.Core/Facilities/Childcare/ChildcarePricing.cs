using RoomBooking.Core.Facilities.Shared;

namespace RoomBooking.Core.Facilities.Childcare;

/// <summary>Prices Childcare orders: multiplier, bulk discount, then VAT.</summary>
public sealed class ChildcarePricing(ChildcarePolicy policy, FacilityTax tax)
{
    public ChildcareQuote Quote(ChildcareItem item, int quantity)
    {
        var baseAmount = item.UnitPrice * quantity * policy.Multiplier;
        var discount = quantity >= policy.BulkThreshold
            ? FacilityMoney.Round(baseAmount * policy.BulkDiscountRate)
            : 0m;
        var handlingFee = 0m;
        var total = tax.Apply(baseAmount - discount + handlingFee);
        return new ChildcareQuote(FacilityMoney.Round(baseAmount), discount, handlingFee, total);
    }
}
