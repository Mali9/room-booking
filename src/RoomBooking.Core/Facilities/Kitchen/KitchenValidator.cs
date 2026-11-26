using RoomBooking.Core.Facilities.Shared;

namespace RoomBooking.Core.Facilities.Kitchen;

/// <summary>Quantity rules of Kitchen orders.</summary>
public sealed class KitchenValidator(KitchenPolicy policy)
{
    public void Validate(KitchenItem item, int quantity)
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
