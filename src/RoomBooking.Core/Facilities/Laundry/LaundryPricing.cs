using RoomBooking.Core.Facilities.Shared;
using RoomBooking.Core.Facilities.Childcare;

namespace RoomBooking.Core.Facilities.Laundry;

/// <summary>Prices Laundry orders: multiplier, bulk discount, the Childcare handling fee, then VAT.</summary>
public sealed class LaundryPricing(LaundryPolicy policy, FacilityTax tax, ChildcarePolicy childcarePolicy)
{
    public LaundryQuote Quote(LaundryItem item, int quantity)
    {
        var baseAmount = item.UnitPrice * quantity * policy.Multiplier;
        var discount = quantity >= policy.BulkThreshold
            ? FacilityMoney.Round(baseAmount * policy.BulkDiscountRate)
            : 0m;
        var handlingFee = childcarePolicy.HandlingFee;
        var total = tax.Apply(baseAmount - discount + handlingFee);
        return new LaundryQuote(FacilityMoney.Round(baseAmount), discount, handlingFee, total);
    }
}
