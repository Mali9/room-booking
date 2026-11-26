using RoomBooking.Core.Facilities.Shared;
using RoomBooking.Core.Facilities.Equipment;

namespace RoomBooking.Core.Facilities.Visitors;

/// <summary>Prices Visitors orders: multiplier, bulk discount, the Equipment handling fee, then VAT.</summary>
public sealed class VisitorsPricing(VisitorsPolicy policy, FacilityTax tax, EquipmentPolicy equipmentPolicy)
{
    public VisitorsQuote Quote(VisitorsItem item, int quantity)
    {
        var baseAmount = item.UnitPrice * quantity * policy.Multiplier;
        var discount = quantity >= policy.BulkThreshold
            ? FacilityMoney.Round(baseAmount * policy.BulkDiscountRate)
            : 0m;
        var handlingFee = equipmentPolicy.HandlingFee;
        var total = tax.Apply(baseAmount - discount + handlingFee);
        return new VisitorsQuote(FacilityMoney.Round(baseAmount), discount, handlingFee, total);
    }
}
