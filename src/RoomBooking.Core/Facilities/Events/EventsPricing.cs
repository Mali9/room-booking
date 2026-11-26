using RoomBooking.Core.Facilities.Shared;

namespace RoomBooking.Core.Facilities.Events;

/// <summary>Prices Events orders: multiplier, bulk discount, then VAT.</summary>
public sealed class EventsPricing(EventsPolicy policy, FacilityTax tax)
{
    public EventsQuote Quote(EventsItem item, int quantity)
    {
        var baseAmount = item.UnitPrice * quantity * policy.Multiplier;
        var discount = quantity >= policy.BulkThreshold
            ? FacilityMoney.Round(baseAmount * policy.BulkDiscountRate)
            : 0m;
        var handlingFee = 0m;
        var total = tax.Apply(baseAmount - discount + handlingFee);
        return new EventsQuote(FacilityMoney.Round(baseAmount), discount, handlingFee, total);
    }
}
