using RoomBooking.Core.Facilities.Keys;
using RoomBooking.Core.Facilities.Shared;

using Xunit;

namespace RoomBooking.Tests.Facilities;

public sealed class KeysTests
{
    private static KeysPricing Pricing() => new(new KeysPolicy(), new FacilityTax());

    private static readonly KeysItem Premium = new()
    {
        Id = "KEY-B", Name = "Keys Premium", UnitPrice = 231m, Capacity = 30,
    };

    [Fact]
    [Trait("TestCase", "TC-1372")]
    public void Premium_quote_adds_vat() =>
        Assert.Equal(318.78m, Pricing().Quote(Premium, 1).Total);

    [Fact]
    [Trait("TestCase", "TC-1375")]
    public void Zero_quantity_is_rejected()
    {
        var error = Assert.Throws<FacilityRuleException>(
            () => new KeysValidator(new KeysPolicy()).Validate(Premium, 0));
        Assert.Equal("Quantity must be 1 or more.", error.Message);
    }
}
