using RoomBooking.Core.Facilities.Shared;

namespace RoomBooking.Core.Facilities.Equipment;

/// <summary>Prices Equipment orders: multiplier, bulk discount, then VAT.</summary>
public sealed class EquipmentPricing(EquipmentPolicy policy, FacilityTax tax)
{
    public EquipmentQuote Quote(EquipmentItem item, int quantity)
    {
        var baseAmount = item.UnitPrice * quantity * policy.Multiplier;
        var discount = quantity >= policy.BulkThreshold
            ? FacilityMoney.Round(baseAmount * policy.BulkDiscountRate)
            : 0m;
        var handlingFee = 0m;
        var total = tax.Apply(baseAmount - discount + handlingFee);
        return new EquipmentQuote(FacilityMoney.Round(baseAmount), discount, handlingFee, total);
    }
}
