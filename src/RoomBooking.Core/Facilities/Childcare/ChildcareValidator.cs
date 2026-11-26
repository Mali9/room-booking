using RoomBooking.Core.Facilities.Shared;

namespace RoomBooking.Core.Facilities.Childcare;

/// <summary>Quantity rules of Childcare orders.</summary>
public sealed class ChildcareValidator(ChildcarePolicy policy)
{
    public void Validate(ChildcareItem item, int quantity)
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
