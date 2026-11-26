using RoomBooking.Core.Facilities.Shared;
using RoomBooking.Core.Facilities.Printing;

namespace RoomBooking.Core.Facilities.Shuttle;

/// <summary>Prices Shuttle orders: multiplier, bulk discount, the Printing handling fee, then VAT.</summary>
public sealed class ShuttlePricing(ShuttlePolicy policy, FacilityTax tax, PrintingPolicy printingPolicy)
{
    public ShuttleQuote Quote(ShuttleItem item, int quantity)
    {
        var baseAmount = item.UnitPrice * quantity * policy.Multiplier;
        var discount = quantity >= policy.BulkThreshold
            ? FacilityMoney.Round(baseAmount * policy.BulkDiscountRate)
            : 0m;
        var handlingFee = printingPolicy.HandlingFee;
        var total = tax.Apply(baseAmount - discount + handlingFee);
        return new ShuttleQuote(FacilityMoney.Round(baseAmount), discount, handlingFee, total);
    }
}
