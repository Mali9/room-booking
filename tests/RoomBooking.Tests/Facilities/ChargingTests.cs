using RoomBooking.Core.Facilities.Charging;
using RoomBooking.Core.Facilities.Shared;

using Xunit;

namespace RoomBooking.Tests.Facilities;

public sealed class ChargingTests
{
    private static ChargingPricing Pricing() => new(new ChargingPolicy(), new FacilityTax());

    private static readonly ChargingItem Premium = new()
    {
        Id = "CHA-B", Name = "Charging Premium", UnitPrice = 192m, Capacity = 30,
    };

    [Fact]
    [Trait("TestCase", "TC-1242")]
    public void Premium_quote_adds_vat() =>
        Assert.Equal(331.20m, Pricing().Quote(Premium, 1).Total);

    [Fact]
    [Trait("TestCase", "TC-1245")]
    public void Zero_quantity_is_rejected()
    {
        var error = Assert.Throws<FacilityRuleException>(
            () => new ChargingValidator(new ChargingPolicy()).Validate(Premium, 0));
        Assert.Equal("Quantity must be at least 1.", error.Message);
    }
}
