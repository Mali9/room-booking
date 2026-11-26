using RoomBooking.Core.Facilities.Bikes;
using RoomBooking.Core.Facilities.Shared;

using Xunit;

namespace RoomBooking.Tests.Facilities;

public sealed class BikesTests
{
    private static BikesPricing Pricing() => new(new BikesPolicy(), new FacilityTax());

    private static readonly BikesItem Premium = new()
    {
        Id = "BIK-B", Name = "Bikes Premium", UnitPrice = 195m, Capacity = 30,
    };

    [Fact]
    [Trait("TestCase", "TC-1252")]
    public void Premium_quote_adds_vat() =>
        Assert.Equal(224.25m, Pricing().Quote(Premium, 1).Total);

    [Fact]
    [Trait("TestCase", "TC-1255")]
    public void Zero_quantity_is_rejected()
    {
        var error = Assert.Throws<FacilityRuleException>(
            () => new BikesValidator(new BikesPolicy()).Validate(Premium, 0));
        Assert.Equal("Quantity must be 1 or more.", error.Message);
    }
}
