using RoomBooking.Core.Facilities.Security;
using RoomBooking.Core.Facilities.Shared;

using Xunit;

namespace RoomBooking.Tests.Facilities;

public sealed class SecurityTests
{
    private static SecurityPricing Pricing() => new(new SecurityPolicy(), new FacilityTax());

    private static readonly SecurityItem Premium = new()
    {
        Id = "SEC-B", Name = "Security Premium", UnitPrice = 138m, Capacity = 30,
    };

    [Fact]
    [Trait("TestCase", "TC-1062")]
    public void Premium_quote_adds_vat() =>
        Assert.Equal(176.09m, Pricing().Quote(Premium, 1).Total);

    [Fact]
    [Trait("TestCase", "TC-1065")]
    public void Zero_quantity_is_rejected()
    {
        var error = Assert.Throws<FacilityRuleException>(
            () => new SecurityValidator(new SecurityPolicy()).Validate(Premium, 0));
        Assert.Equal("Quantity must be at least 1.", error.Message);
    }
}
