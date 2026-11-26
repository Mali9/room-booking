using RoomBooking.Core.Facilities.Shared;

namespace RoomBooking.Core.Facilities.Gym;

/// <summary>Prices Gym orders: multiplier, bulk discount, then VAT.</summary>
public sealed class GymPricing(GymPolicy policy, FacilityTax tax)
{
    public GymQuote Quote(GymItem item, int quantity)
    {
        var baseAmount = item.UnitPrice * quantity * policy.Multiplier;
        var discount = quantity >= policy.BulkThreshold
            ? FacilityMoney.Round(baseAmount * policy.BulkDiscountRate)
            : 0m;
        var handlingFee = 0m;
        var total = tax.Apply(baseAmount - discount + handlingFee);
        return new GymQuote(FacilityMoney.Round(baseAmount), discount, handlingFee, total);
    }
}
