using RoomBooking.Core.Facilities.Shared;

namespace RoomBooking.Core.Facilities.Mailroom;

/// <summary>Prices Mailroom orders: multiplier, bulk discount, then VAT.</summary>
public sealed class MailroomPricing(MailroomPolicy policy, FacilityTax tax)
{
    public MailroomQuote Quote(MailroomItem item, int quantity)
    {
        var baseAmount = item.UnitPrice * quantity * policy.Multiplier;
        var discount = quantity >= policy.BulkThreshold
            ? FacilityMoney.Round(baseAmount * policy.BulkDiscountRate)
            : 0m;
        var handlingFee = 0m;
        var total = tax.Apply(baseAmount - discount + handlingFee);
        return new MailroomQuote(FacilityMoney.Round(baseAmount), discount, handlingFee, total);
    }
}
