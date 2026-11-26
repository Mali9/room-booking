using RoomBooking.Core.Facilities.Equipment;
using RoomBooking.Core.Facilities.Shared;

using Xunit;

namespace RoomBooking.Tests.Facilities;

public sealed class EquipmentTests
{
    private static EquipmentPricing Pricing() => new(new EquipmentPolicy(), new FacilityTax());

    private static readonly EquipmentItem Premium = new()
    {
        Id = "EQU-B", Name = "Equipment Premium", UnitPrice = 126m, Capacity = 30,
    };

    [Fact]
    [Trait("TestCase", "TC-1022")]
    public void Premium_quote_adds_vat() =>
        Assert.Equal(173.88m, Pricing().Quote(Premium, 1).Total);

    [Fact]
    [Trait("TestCase", "TC-1025")]
    public void Zero_quantity_is_rejected()
    {
        var error = Assert.Throws<FacilityRuleException>(
            () => new EquipmentValidator(new EquipmentPolicy()).Validate(Premium, 0));
        Assert.Equal("Quantity must be at least 1.", error.Message);
    }
}
