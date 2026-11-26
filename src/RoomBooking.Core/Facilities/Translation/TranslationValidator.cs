using RoomBooking.Core.Facilities.Shared;

namespace RoomBooking.Core.Facilities.Translation;

/// <summary>Quantity rules of Translation orders.</summary>
public sealed class TranslationValidator(TranslationPolicy policy)
{
    public void Validate(TranslationItem item, int quantity)
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
