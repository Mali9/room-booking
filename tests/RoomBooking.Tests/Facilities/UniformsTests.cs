using RoomBooking.Core.Facilities.Uniforms;
using RoomBooking.Core.Facilities.Shared;

using Xunit;

namespace RoomBooking.Tests.Facilities;

public sealed class UniformsTests
{
    private static UniformsPricing Pricing() => new(new UniformsPolicy(), new FacilityTax());

    private static readonly UniformsItem Premium = new()
    {
        Id = "UNI-B", Name = "Uniforms Premium", UnitPrice = 228m, Capacity = 30,
    };

    [Fact]
    [Trait("TestCase", "TC-1362")]
    public void Premium_quote_adds_vat() =>
        Assert.Equal(290.93m, Pricing().Quote(Premium, 1).Total);

    [Fact]
    [Trait("TestCase", "TC-1365")]
    public void Zero_quantity_is_rejected()
    {
        var error = Assert.Throws<FacilityRuleException>(
            () => new UniformsValidator(new UniformsPolicy()).Validate(Premium, 0));
        Assert.Equal("Quantity must be at least 1.", error.Message);
    }
}
