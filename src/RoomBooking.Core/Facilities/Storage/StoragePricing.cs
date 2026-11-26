using RoomBooking.Core.Facilities.Shared;

namespace RoomBooking.Core.Facilities.Storage;

/// <summary>Prices Storage orders: multiplier, bulk discount, then VAT.</summary>
public sealed class StoragePricing(StoragePolicy policy, FacilityTax tax)
{
    public StorageQuote Quote(StorageItem item, int quantity)
    {
        var baseAmount = item.UnitPrice * quantity * policy.Multiplier;
        var discount = quantity >= policy.BulkThreshold
            ? FacilityMoney.Round(baseAmount * policy.BulkDiscountRate)
            : 0m;
        var handlingFee = 0m;
        var total = tax.Apply(baseAmount - discount + handlingFee);
        return new StorageQuote(FacilityMoney.Round(baseAmount), discount, handlingFee, total);
    }
}
