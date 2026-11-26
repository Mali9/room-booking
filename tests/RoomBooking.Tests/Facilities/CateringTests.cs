using RoomBooking.Core.Facilities.Catering;
using RoomBooking.Core.Facilities.Shared;

using Xunit;

namespace RoomBooking.Tests.Facilities;

public sealed class CateringTests
{
    private static CateringPricing Pricing() => new(new CateringPolicy(), new FacilityTax());

    private static readonly CateringItem Premium = new()
    {
        Id = "CAT-B", Name = "Catering Premium", UnitPrice = 120m, Capacity = 30,
    };

    [Fact]
    [Trait("TestCase", "TC-1002")]
    public void Premium_quote_adds_vat() =>
        Assert.Equal(138.00m, Pricing().Quote(Premium, 1).Total);

    [Fact]
    [Trait("TestCase", "TC-1005")]
    public void Zero_quantity_is_rejected()
    {
        var error = Assert.Throws<FacilityRuleException>(
            () => new CateringValidator(new CateringPolicy()).Validate(Premium, 0));
        Assert.Equal("Quantity must be at least 1.", error.Message);
    }
}
