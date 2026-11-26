using RoomBooking.Core.Facilities.Shuttle;
using RoomBooking.Core.Facilities.Shared;
using RoomBooking.Core.Facilities.Printing;
using Xunit;

namespace RoomBooking.Tests.Facilities;

public sealed class ShuttleTests
{
    private static ShuttlePricing Pricing() => new(new ShuttlePolicy(), new FacilityTax(), new PrintingPolicy());

    private static readonly ShuttleItem Premium = new()
    {
        Id = "SHU-B", Name = "Shuttle Premium", UnitPrice = 153m, Capacity = 30,
    };

    [Fact]
    [Trait("TestCase", "TC-1112")]
    public void Premium_quote_adds_vat() =>
        Assert.Equal(215.53m, Pricing().Quote(Premium, 1).Total);

    [Fact]
    [Trait("TestCase", "TC-1115")]
    public void Zero_quantity_is_rejected()
    {
        var error = Assert.Throws<FacilityRuleException>(
            () => new ShuttleValidator(new ShuttlePolicy()).Validate(Premium, 0));
        Assert.Equal("Quantity must be at least 1.", error.Message);
    }
}
