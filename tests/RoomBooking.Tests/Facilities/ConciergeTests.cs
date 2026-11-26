using RoomBooking.Core.Facilities.Concierge;
using RoomBooking.Core.Facilities.Shared;

using Xunit;

namespace RoomBooking.Tests.Facilities;

public sealed class ConciergeTests
{
    private static ConciergePricing Pricing() => new(new ConciergePolicy(), new FacilityTax());

    private static readonly ConciergeItem Premium = new()
    {
        Id = "CON-B", Name = "Concierge Premium", UnitPrice = 222m, Capacity = 30,
    };

    [Fact]
    [Trait("TestCase", "TC-1342")]
    public void Premium_quote_adds_vat() =>
        Assert.Equal(382.95m, Pricing().Quote(Premium, 1).Total);

    [Fact]
    [Trait("TestCase", "TC-1345")]
    public void Zero_quantity_is_rejected()
    {
        var error = Assert.Throws<FacilityRuleException>(
            () => new ConciergeValidator(new ConciergePolicy()).Validate(Premium, 0));
        Assert.Equal("Quantity must be 1 or more.", error.Message);
    }
}
