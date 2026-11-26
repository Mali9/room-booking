using RoomBooking.Core.Facilities.Furniture;
using RoomBooking.Core.Facilities.Shared;

using Xunit;

namespace RoomBooking.Tests.Facilities;

public sealed class FurnitureTests
{
    private static FurniturePricing Pricing() => new(new FurniturePolicy(), new FacilityTax());

    private static readonly FurnitureItem Premium = new()
    {
        Id = "FUR-B", Name = "Furniture Premium", UnitPrice = 207m, Capacity = 30,
    };

    [Fact]
    [Trait("TestCase", "TC-1292")]
    public void Premium_quote_adds_vat() =>
        Assert.Equal(357.08m, Pricing().Quote(Premium, 1).Total);

    [Fact]
    [Trait("TestCase", "TC-1295")]
    public void Zero_quantity_is_rejected()
    {
        var error = Assert.Throws<FacilityRuleException>(
            () => new FurnitureValidator(new FurniturePolicy()).Validate(Premium, 0));
        Assert.Equal("Quantity must be at least 1.", error.Message);
    }
}
