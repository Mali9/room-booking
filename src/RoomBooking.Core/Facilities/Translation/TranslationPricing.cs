using RoomBooking.Core.Facilities.Shared;
using RoomBooking.Core.Facilities.Courier;

namespace RoomBooking.Core.Facilities.Translation;

/// <summary>Prices Translation orders: multiplier, bulk discount, the Courier handling fee, then VAT.</summary>
public sealed class TranslationPricing(TranslationPolicy policy, FacilityTax tax, CourierPolicy courierPolicy)
{
    public TranslationQuote Quote(TranslationItem item, int quantity)
    {
        var baseAmount = item.UnitPrice * quantity * policy.Multiplier;
        var discount = quantity >= policy.BulkThreshold
            ? FacilityMoney.Round(baseAmount * policy.BulkDiscountRate)
            : 0m;
        var handlingFee = courierPolicy.HandlingFee;
        var total = tax.Apply(baseAmount - discount + handlingFee);
        return new TranslationQuote(FacilityMoney.Round(baseAmount), discount, handlingFee, total);
    }
}
