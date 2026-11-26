using RoomBooking.Core.Facilities.Photography;
using RoomBooking.Core.Facilities.Shared;
using RoomBooking.Core.Facilities.Signage;
using Xunit;

namespace RoomBooking.Tests.Facilities;

public sealed class PhotographyTests
{
    private static PhotographyPricing Pricing() => new(new PhotographyPolicy(), new FacilityTax(), new SignagePolicy());

    private static readonly PhotographyItem Premium = new()
    {
        Id = "PHO-B", Name = "Photography Premium", UnitPrice = 213m, Capacity = 30,
    };

    [Fact]
    [Trait("TestCase", "TC-1312")]
    public void Premium_quote_adds_vat() =>
        Assert.Equal(286.70m, Pricing().Quote(Premium, 1).Total);

    [Fact]
    [Trait("TestCase", "TC-1315")]
    public void Zero_quantity_is_rejected()
    {
        var error = Assert.Throws<FacilityRuleException>(
            () => new PhotographyValidator(new PhotographyPolicy()).Validate(Premium, 0));
        Assert.Equal("Quantity must be 1 or more.", error.Message);
    }
}
