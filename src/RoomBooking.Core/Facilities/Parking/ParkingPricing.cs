using RoomBooking.Core.Facilities.Shared;

namespace RoomBooking.Core.Facilities.Parking;

/// <summary>Prices Parking orders: multiplier, bulk discount, then VAT.</summary>
public sealed class ParkingPricing(ParkingPolicy policy, FacilityTax tax)
{
    public ParkingQuote Quote(ParkingItem item, int quantity)
    {
        var baseAmount = item.UnitPrice * quantity * policy.Multiplier;
        var discount = quantity >= policy.BulkThreshold
            ? FacilityMoney.Round(baseAmount * policy.BulkDiscountRate)
            : 0m;
        var handlingFee = 0m;
        var total = tax.Apply(baseAmount - discount + handlingFee);
        return new ParkingQuote(FacilityMoney.Round(baseAmount), discount, handlingFee, total);
    }
}
