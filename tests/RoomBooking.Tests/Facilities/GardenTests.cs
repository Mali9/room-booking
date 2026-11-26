using RoomBooking.Core.Facilities.Garden;
using RoomBooking.Core.Facilities.Shared;
using RoomBooking.Core.Facilities.Kitchen;
using Xunit;

namespace RoomBooking.Tests.Facilities;

public sealed class GardenTests
{
    private static GardenPricing Pricing() => new(new GardenPolicy(), new FacilityTax(), new KitchenPolicy());

    private static readonly GardenItem Premium = new()
    {
        Id = "GAR-B", Name = "Garden Premium", UnitPrice = 189m, Capacity = 30,
    };

    [Fact]
    [Trait("TestCase", "TC-1232")]
    public void Premium_quote_adds_vat() =>
        Assert.Equal(286.06m, Pricing().Quote(Premium, 1).Total);

    [Fact]
    [Trait("TestCase", "TC-1235")]
    public void Zero_quantity_is_rejected()
    {
        var error = Assert.Throws<FacilityRuleException>(
            () => new GardenValidator(new GardenPolicy()).Validate(Premium, 0));
        Assert.Equal("Quantity must be 1 or more.", error.Message);
    }
}
