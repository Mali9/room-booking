using RoomBooking.Core.Facilities.Signage;
using RoomBooking.Core.Facilities.Shared;

using Xunit;

namespace RoomBooking.Tests.Facilities;

public sealed class SignageTests
{
    private static SignagePricing Pricing() => new(new SignagePolicy(), new FacilityTax());

    private static readonly SignageItem Premium = new()
    {
        Id = "SIG-B", Name = "Signage Premium", UnitPrice = 210m, Capacity = 30,
    };

    [Fact]
    [Trait("TestCase", "TC-1302")]
    public void Premium_quote_adds_vat() =>
        Assert.Equal(243.60m, Pricing().Quote(Premium, 1).Total);

    [Fact]
    [Trait("TestCase", "TC-1305")]
    public void Zero_quantity_is_rejected()
    {
        var error = Assert.Throws<FacilityRuleException>(
            () => new SignageValidator(new SignagePolicy()).Validate(Premium, 0));
        Assert.Equal("Quantity must be at least 1.", error.Message);
    }
}
