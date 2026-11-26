using RoomBooking.Core.Facilities.Lounge;
using RoomBooking.Core.Facilities.Shared;

using Xunit;

namespace RoomBooking.Tests.Facilities;

public sealed class LoungeTests
{
    private static LoungePricing Pricing() => new(new LoungePolicy(), new FacilityTax());

    private static readonly LoungeItem Premium = new()
    {
        Id = "LOU-B", Name = "Lounge Premium", UnitPrice = 156m, Capacity = 30,
    };

    [Fact]
    [Trait("TestCase", "TC-1122")]
    public void Premium_quote_adds_vat() =>
        Assert.Equal(215.28m, Pricing().Quote(Premium, 1).Total);

    [Fact]
    [Trait("TestCase", "TC-1125")]
    public void Zero_quantity_is_rejected()
    {
        var error = Assert.Throws<FacilityRuleException>(
            () => new LoungeValidator(new LoungePolicy()).Validate(Premium, 0));
        Assert.Equal("Quantity must be at least 1.", error.Message);
    }
}
