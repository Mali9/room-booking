using RoomBooking.Core.Facilities.Shared;

namespace RoomBooking.Core.Facilities.Studio;

/// <summary>Quantity rules of Studio orders.</summary>
public sealed class StudioValidator(StudioPolicy policy)
{
    public void Validate(StudioItem item, int quantity)
    {
        if (quantity < 1)
        {
            throw new FacilityRuleException("Quantity must be at least 1.");
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
