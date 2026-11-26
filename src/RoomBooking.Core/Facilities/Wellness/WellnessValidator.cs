using RoomBooking.Core.Facilities.Shared;

namespace RoomBooking.Core.Facilities.Wellness;

/// <summary>Quantity rules of Wellness orders.</summary>
public sealed class WellnessValidator(WellnessPolicy policy)
{
    public void Validate(WellnessItem item, int quantity)
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
