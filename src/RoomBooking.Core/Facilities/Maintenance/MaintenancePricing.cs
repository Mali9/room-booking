using RoomBooking.Core.Facilities.Shared;

namespace RoomBooking.Core.Facilities.Maintenance;

/// <summary>Prices Maintenance orders: multiplier, bulk discount, then VAT.</summary>
public sealed class MaintenancePricing(MaintenancePolicy policy, FacilityTax tax)
{
    public MaintenanceQuote Quote(MaintenanceItem item, int quantity)
    {
        var baseAmount = item.UnitPrice * quantity * policy.Multiplier;
        var discount = quantity >= policy.BulkThreshold
            ? FacilityMoney.Round(baseAmount * policy.BulkDiscountRate)
            : 0m;
        var handlingFee = 0m;
        var total = tax.Apply(baseAmount - discount + handlingFee);
        return new MaintenanceQuote(FacilityMoney.Round(baseAmount), discount, handlingFee, total);
    }
}
