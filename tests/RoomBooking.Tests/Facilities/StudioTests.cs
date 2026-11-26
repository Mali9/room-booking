using RoomBooking.Core.Facilities.Studio;
using RoomBooking.Core.Facilities.Shared;

using Xunit;

namespace RoomBooking.Tests.Facilities;

public sealed class StudioTests
{
    private static StudioPricing Pricing() => new(new StudioPolicy(), new FacilityTax());

    private static readonly StudioItem Premium = new()
    {
        Id = "STU-B", Name = "Studio Premium", UnitPrice = 183m, Capacity = 30,
    };

    [Fact]
    [Trait("TestCase", "TC-1212")]
    public void Premium_quote_adds_vat() =>
        Assert.Equal(231.50m, Pricing().Quote(Premium, 1).Total);

    [Fact]
    [Trait("TestCase", "TC-1215")]
    public void Zero_quantity_is_rejected()
    {
        var error = Assert.Throws<FacilityRuleException>(
            () => new StudioValidator(new StudioPolicy()).Validate(Premium, 0));
        Assert.Equal("Quantity must be 1 or more.", error.Message);
    }
}
