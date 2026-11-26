using RoomBooking.Core.Facilities.Pantry;
using RoomBooking.Core.Facilities.Shared;
using RoomBooking.Core.Facilities.Concierge;
using Xunit;

namespace RoomBooking.Tests.Facilities;

public sealed class PantryTests
{
    private static PantryPricing Pricing() => new(new PantryPolicy(), new FacilityTax(), new ConciergePolicy());

    private static readonly PantryItem Premium = new()
    {
        Id = "PAN-B", Name = "Pantry Premium", UnitPrice = 225m, Capacity = 30,
    };

    [Fact]
    [Trait("TestCase", "TC-1352")]
    public void Premium_quote_adds_vat() =>
        Assert.Equal(287.50m, Pricing().Quote(Premium, 1).Total);

    [Fact]
    [Trait("TestCase", "TC-1355")]
    public void Zero_quantity_is_rejected()
    {
        var error = Assert.Throws<FacilityRuleException>(
            () => new PantryValidator(new PantryPolicy()).Validate(Premium, 0));
        Assert.Equal("Quantity must be at least 1.", error.Message);
    }
}
