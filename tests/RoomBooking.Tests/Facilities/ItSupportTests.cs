using RoomBooking.Core.Facilities.ItSupport;
using RoomBooking.Core.Facilities.Shared;

using Xunit;

namespace RoomBooking.Tests.Facilities;

public sealed class ItSupportTests
{
    private static ItSupportPricing Pricing() => new(new ItSupportPolicy(), new FacilityTax());

    private static readonly ItSupportItem Premium = new()
    {
        Id = "ITS-B", Name = "ItSupport Premium", UnitPrice = 204m, Capacity = 30,
    };

    [Fact]
    [Trait("TestCase", "TC-1282")]
    public void Premium_quote_adds_vat() =>
        Assert.Equal(295.80m, Pricing().Quote(Premium, 1).Total);

    [Fact]
    [Trait("TestCase", "TC-1285")]
    public void Zero_quantity_is_rejected()
    {
        var error = Assert.Throws<FacilityRuleException>(
            () => new ItSupportValidator(new ItSupportPolicy()).Validate(Premium, 0));
        Assert.Equal("Quantity must be at least 1.", error.Message);
    }
}
