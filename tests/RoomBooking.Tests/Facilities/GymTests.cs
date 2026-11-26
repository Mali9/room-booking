using RoomBooking.Core.Facilities.Gym;
using RoomBooking.Core.Facilities.Shared;

using Xunit;

namespace RoomBooking.Tests.Facilities;

public sealed class GymTests
{
    private static GymPricing Pricing() => new(new GymPolicy(), new FacilityTax());

    private static readonly GymItem Premium = new()
    {
        Id = "GYM-B", Name = "Gym Premium", UnitPrice = 159m, Capacity = 30,
    };

    [Fact]
    [Trait("TestCase", "TC-1132")]
    public void Premium_quote_adds_vat() =>
        Assert.Equal(230.55m, Pricing().Quote(Premium, 1).Total);

    [Fact]
    [Trait("TestCase", "TC-1135")]
    public void Zero_quantity_is_rejected()
    {
        var error = Assert.Throws<FacilityRuleException>(
            () => new GymValidator(new GymPolicy()).Validate(Premium, 0));
        Assert.Equal("Quantity must be at least 1.", error.Message);
    }
}
