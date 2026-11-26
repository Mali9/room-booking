using RoomBooking.Core.Facilities.Wellness;
using RoomBooking.Core.Facilities.Shared;

using Xunit;

namespace RoomBooking.Tests.Facilities;

public sealed class WellnessTests
{
    private static WellnessPricing Pricing() => new(new WellnessPolicy(), new FacilityTax());

    private static readonly WellnessItem Premium = new()
    {
        Id = "WEL-B", Name = "Wellness Premium", UnitPrice = 174m, Capacity = 30,
    };

    [Fact]
    [Trait("TestCase", "TC-1182")]
    public void Premium_quote_adds_vat() =>
        Assert.Equal(250.13m, Pricing().Quote(Premium, 1).Total);

    [Fact]
    [Trait("TestCase", "TC-1185")]
    public void Zero_quantity_is_rejected()
    {
        var error = Assert.Throws<FacilityRuleException>(
            () => new WellnessValidator(new WellnessPolicy()).Validate(Premium, 0));
        Assert.Equal("Quantity must be at least 1.", error.Message);
    }
}
