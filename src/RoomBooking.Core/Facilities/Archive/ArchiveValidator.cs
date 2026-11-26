using RoomBooking.Core.Facilities.Shared;

namespace RoomBooking.Core.Facilities.Archive;

/// <summary>Quantity rules of Archive orders.</summary>
public sealed class ArchiveValidator(ArchivePolicy policy)
{
    public void Validate(ArchiveItem item, int quantity)
    {
        if (quantity < 1)
        {
            throw new FacilityRuleException("Quantity must be 1 or more.");
        }

        if (quantity > policy.MaxQuantity)
        {
            throw new FacilityRuleException($"Quantity cannot exceed {policy.MaxQuantity}.");
        }

        if (quantity > item.Capacity)
        {
            throw new FacilityRuleException($"Only {item.Capacity} available for {item.Name}.");
        }
    }
}
