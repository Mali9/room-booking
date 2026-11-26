using RoomBooking.Core.Facilities.Shared;

namespace RoomBooking.Core.Facilities.Badges;

/// <summary>Quantity rules of Badges orders.</summary>
public sealed class BadgesValidator(BadgesPolicy policy)
{
    public void Validate(BadgesItem item, int quantity)
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
