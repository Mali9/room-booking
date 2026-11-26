using RoomBooking.Core.Facilities.Badges;
using RoomBooking.Core.Facilities.Shared;

using Xunit;

namespace RoomBooking.Tests.Facilities;

public sealed class BadgesTests
{
    private static BadgesPricing Pricing() => new(new BadgesPolicy(), new FacilityTax());

    private static readonly BadgesItem Premium = new()
    {
        Id = "BAD-B", Name = "Badges Premium", UnitPrice = 234m, Capacity = 30,
    };

    [Fact]
    [Trait("TestCase", "TC-1382")]
    public void Premium_quote_adds_vat() =>
        Assert.Equal(339.30m, Pricing().Quote(Premium, 1).Total);

    [Fact]
    [Trait("TestCase", "TC-1385")]
    public void Zero_quantity_is_rejected()
    {
        var error = Assert.Throws<FacilityRuleException>(
            () => new BadgesValidator(new BadgesPolicy()).Validate(Premium, 0));
        Assert.Equal("Quantity must be at least 1.", error.Message);
    }
}
