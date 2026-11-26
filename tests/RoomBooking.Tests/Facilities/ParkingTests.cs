using RoomBooking.Core.Facilities.Parking;
using RoomBooking.Core.Facilities.Shared;

using Xunit;

namespace RoomBooking.Tests.Facilities;

public sealed class ParkingTests
{
    private static ParkingPricing Pricing() => new(new ParkingPolicy(), new FacilityTax());

    private static readonly ParkingItem Premium = new()
    {
        Id = "PAR-B", Name = "Parking Premium", UnitPrice = 123m, Capacity = 30,
    };

    [Fact]
    [Trait("TestCase", "TC-1012")]
    public void Premium_quote_adds_vat() =>
        Assert.Equal(183.89m, Pricing().Quote(Premium, 1).Total);

    [Fact]
    [Trait("TestCase", "TC-1015")]
    public void Zero_quantity_is_rejected()
    {
        var error = Assert.Throws<FacilityRuleException>(
            () => new ParkingValidator(new ParkingPolicy()).Validate(Premium, 0));
        Assert.Equal("Quantity must be at least 1.", error.Message);
    }
}
