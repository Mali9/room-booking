using RoomBooking.Core.Facilities.Shared;

namespace RoomBooking.Core.Facilities.Training;

/// <summary>Prices Training orders: multiplier, bulk discount, then VAT.</summary>
public sealed class TrainingPricing(TrainingPolicy policy, FacilityTax tax)
{
    public TrainingQuote Quote(TrainingItem item, int quantity)
    {
        var baseAmount = item.UnitPrice * quantity * policy.Multiplier;
        var discount = quantity >= policy.BulkThreshold
            ? FacilityMoney.Round(baseAmount * policy.BulkDiscountRate)
            : 0m;
        var handlingFee = 0m;
        var total = tax.Apply(baseAmount - discount + handlingFee);
        return new TrainingQuote(FacilityMoney.Round(baseAmount), discount, handlingFee, total);
    }
}
