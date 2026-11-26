using RoomBooking.Core.Facilities.Shared;

namespace RoomBooking.Core.Facilities.Bikes;

/// <summary>Prices Bikes orders: multiplier, bulk discount, then VAT.</summary>
public sealed class BikesPricing(BikesPolicy policy, FacilityTax tax)
{
    public BikesQuote Quote(BikesItem item, int quantity)
    {
        var baseAmount = item.UnitPrice * quantity * policy.Multiplier;
        var discount = quantity >= policy.BulkThreshold
            ? FacilityMoney.Round(baseAmount * policy.BulkDiscountRate)
            : 0m;
        var handlingFee = 0m;
        var total = tax.Apply(baseAmount - discount + handlingFee);
        return new BikesQuote(FacilityMoney.Round(baseAmount), discount, handlingFee, total);
    }
}
