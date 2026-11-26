using RoomBooking.Core.Facilities.Shared;
using RoomBooking.Core.Facilities.Wellness;

namespace RoomBooking.Core.Facilities.Library;

/// <summary>Prices Library orders: multiplier, bulk discount, the Wellness handling fee, then VAT.</summary>
public sealed class LibraryPricing(LibraryPolicy policy, FacilityTax tax, WellnessPolicy wellnessPolicy)
{
    public LibraryQuote Quote(LibraryItem item, int quantity)
    {
        var baseAmount = item.UnitPrice * quantity * policy.Multiplier;
        var discount = quantity >= policy.BulkThreshold
            ? FacilityMoney.Round(baseAmount * policy.BulkDiscountRate)
            : 0m;
        var handlingFee = wellnessPolicy.HandlingFee;
        var total = tax.Apply(baseAmount - discount + handlingFee);
        return new LibraryQuote(FacilityMoney.Round(baseAmount), discount, handlingFee, total);
    }
}
