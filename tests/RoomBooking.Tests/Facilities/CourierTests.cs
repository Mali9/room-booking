using RoomBooking.Core.Facilities.Courier;
using RoomBooking.Core.Facilities.Shared;

using Xunit;

namespace RoomBooking.Tests.Facilities;

public sealed class CourierTests
{
    private static CourierPricing Pricing() => new(new CourierPolicy(), new FacilityTax());

    private static readonly CourierItem Premium = new()
    {
        Id = "COU-B", Name = "Courier Premium", UnitPrice = 198m, Capacity = 30,
    };

    [Fact]
    [Trait("TestCase", "TC-1262")]
    public void Premium_quote_adds_vat() =>
        Assert.Equal(252.65m, Pricing().Quote(Premium, 1).Total);

    [Fact]
    [Trait("TestCase", "TC-1265")]
    public void Zero_quantity_is_rejected()
    {
        var error = Assert.Throws<FacilityRuleException>(
            () => new CourierValidator(new CourierPolicy()).Validate(Premium, 0));
        Assert.Equal("Quantity must be at least 1.", error.Message);
    }
}
