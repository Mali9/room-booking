using RoomBooking.Core.Facilities.Desks;
using RoomBooking.Core.Facilities.Shared;

using Xunit;

namespace RoomBooking.Tests.Facilities;

public sealed class DesksTests
{
    private static DesksPricing Pricing() => new(new DesksPolicy(), new FacilityTax());

    private static readonly DesksItem Premium = new()
    {
        Id = "DES-B", Name = "Desks Premium", UnitPrice = 144m, Capacity = 30,
    };

    [Fact]
    [Trait("TestCase", "TC-1082")]
    public void Premium_quote_adds_vat() =>
        Assert.Equal(208.80m, Pricing().Quote(Premium, 1).Total);

    [Fact]
    [Trait("TestCase", "TC-1085")]
    public void Zero_quantity_is_rejected()
    {
        var error = Assert.Throws<FacilityRuleException>(
            () => new DesksValidator(new DesksPolicy()).Validate(Premium, 0));
        Assert.Equal("Quantity must be at least 1.", error.Message);
    }
}
