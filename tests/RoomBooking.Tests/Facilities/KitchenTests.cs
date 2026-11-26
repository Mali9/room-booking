using RoomBooking.Core.Facilities.Kitchen;
using RoomBooking.Core.Facilities.Shared;

using Xunit;

namespace RoomBooking.Tests.Facilities;

public sealed class KitchenTests
{
    private static KitchenPricing Pricing() => new(new KitchenPolicy(), new FacilityTax());

    private static readonly KitchenItem Premium = new()
    {
        Id = "KIT-B", Name = "Kitchen Premium", UnitPrice = 186m, Capacity = 30,
    };

    [Fact]
    [Trait("TestCase", "TC-1222")]
    public void Premium_quote_adds_vat() =>
        Assert.Equal(256.68m, Pricing().Quote(Premium, 1).Total);

    [Fact]
    [Trait("TestCase", "TC-1225")]
    public void Zero_quantity_is_rejected()
    {
        var error = Assert.Throws<FacilityRuleException>(
            () => new KitchenValidator(new KitchenPolicy()).Validate(Premium, 0));
        Assert.Equal("Quantity must be 1 or more.", error.Message);
    }
}
