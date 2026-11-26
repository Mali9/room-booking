using RoomBooking.Core.Facilities.Labs;
using RoomBooking.Core.Facilities.Shared;

using Xunit;

namespace RoomBooking.Tests.Facilities;

public sealed class LabsTests
{
    private static LabsPricing Pricing() => new(new LabsPolicy(), new FacilityTax());

    private static readonly LabsItem Premium = new()
    {
        Id = "LAB-B", Name = "Labs Premium", UnitPrice = 180m, Capacity = 30,
    };

    [Fact]
    [Trait("TestCase", "TC-1202")]
    public void Premium_quote_adds_vat() =>
        Assert.Equal(208.80m, Pricing().Quote(Premium, 1).Total);

    [Fact]
    [Trait("TestCase", "TC-1205")]
    public void Zero_quantity_is_rejected()
    {
        var error = Assert.Throws<FacilityRuleException>(
            () => new LabsValidator(new LabsPolicy()).Validate(Premium, 0));
        Assert.Equal("Quantity must be at least 1.", error.Message);
    }
}
