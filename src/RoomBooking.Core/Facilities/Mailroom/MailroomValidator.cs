using RoomBooking.Core.Facilities.Shared;

namespace RoomBooking.Core.Facilities.Mailroom;

/// <summary>Quantity rules of Mailroom orders.</summary>
public sealed class MailroomValidator(MailroomPolicy policy)
{
    public void Validate(MailroomItem item, int quantity)
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
