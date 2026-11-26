using RoomBooking.Core.Facilities.Shared;

namespace RoomBooking.Core.Facilities.Lockers;

/// <summary>Quantity rules of Lockers orders.</summary>
public sealed class LockersValidator(LockersPolicy policy)
{
    public void Validate(LockersItem item, int quantity)
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
