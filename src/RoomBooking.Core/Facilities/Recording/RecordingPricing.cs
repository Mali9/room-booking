using RoomBooking.Core.Facilities.Shared;

namespace RoomBooking.Core.Facilities.Recording;

/// <summary>Prices Recording orders: multiplier, bulk discount, then VAT.</summary>
public sealed class RecordingPricing(RecordingPolicy policy, FacilityTax tax)
{
    public RecordingQuote Quote(RecordingItem item, int quantity)
    {
        var baseAmount = item.UnitPrice * quantity * policy.Multiplier;
        var discount = quantity >= policy.BulkThreshold
            ? FacilityMoney.Round(baseAmount * policy.BulkDiscountRate)
            : 0m;
        var handlingFee = 0m;
        var total = tax.Apply(baseAmount - discount + handlingFee);
        return new RecordingQuote(FacilityMoney.Round(baseAmount), discount, handlingFee, total);
    }
}
