using RoomBooking.Core.Facilities.Shared;
using RoomBooking.Core.Facilities.Signage;

namespace RoomBooking.Core.Facilities.Photography;

/// <summary>Prices Photography orders: multiplier, bulk discount, the Signage handling fee, then VAT.</summary>
public sealed class PhotographyPricing(PhotographyPolicy policy, FacilityTax tax, SignagePolicy signagePolicy)
{
    public PhotographyQuote Quote(PhotographyItem item, int quantity)
    {
        var baseAmount = item.UnitPrice * quantity * policy.Multiplier;
        var discount = quantity >= policy.BulkThreshold
            ? FacilityMoney.Round(baseAmount * policy.BulkDiscountRate)
            : 0m;
        var handlingFee = signagePolicy.HandlingFee;
        var total = tax.Apply(baseAmount - discount + handlingFee);
        return new PhotographyQuote(FacilityMoney.Round(baseAmount), discount, handlingFee, total);
    }
}
