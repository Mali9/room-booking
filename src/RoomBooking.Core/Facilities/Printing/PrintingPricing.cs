using RoomBooking.Core.Facilities.Shared;

namespace RoomBooking.Core.Facilities.Printing;

/// <summary>Prices Printing orders: multiplier, bulk discount, then VAT.</summary>
public sealed class PrintingPricing(PrintingPolicy policy, FacilityTax tax)
{
    public PrintingQuote Quote(PrintingItem item, int quantity)
    {
        var baseAmount = item.UnitPrice * quantity * policy.Multiplier;
        var discount = quantity >= policy.BulkThreshold
            ? FacilityMoney.Round(baseAmount * policy.BulkDiscountRate)
            : 0m;
        var handlingFee = 0m;
        var total = tax.Apply(baseAmount - discount + handlingFee);
        return new PrintingQuote(FacilityMoney.Round(baseAmount), discount, handlingFee, total);
    }
}
