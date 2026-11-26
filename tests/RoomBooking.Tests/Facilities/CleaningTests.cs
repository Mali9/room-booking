using RoomBooking.Core.Facilities.Cleaning;
using RoomBooking.Core.Facilities.Shared;

using Xunit;

namespace RoomBooking.Tests.Facilities;

public sealed class CleaningTests
{
    private static CleaningPricing Pricing() => new(new CleaningPolicy(), new FacilityTax());

    private static readonly CleaningItem Premium = new()
    {
        Id = "CLE-B", Name = "Cleaning Premium", UnitPrice = 132m, Capacity = 30,
    };

    [Fact]
    [Trait("TestCase", "TC-1042")]
    public void Premium_quote_adds_vat() =>
        Assert.Equal(229.68m, Pricing().Quote(Premium, 1).Total);

    [Fact]
    [Trait("TestCase", "TC-1045")]
    public void Zero_quantity_is_rejected()
    {
        var error = Assert.Throws<FacilityRuleException>(
            () => new CleaningValidator(new CleaningPolicy()).Validate(Premium, 0));
        Assert.Equal("Quantity must be at least 1.", error.Message);
    }
}
