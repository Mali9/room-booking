using RoomBooking.Core.Facilities.Printing;
using RoomBooking.Core.Facilities.Shared;

using Xunit;

namespace RoomBooking.Tests.Facilities;

public sealed class PrintingTests
{
    private static PrintingPricing Pricing() => new(new PrintingPolicy(), new FacilityTax());

    private static readonly PrintingItem Premium = new()
    {
        Id = "PRI-B", Name = "Printing Premium", UnitPrice = 150m, Capacity = 30,
    };

    [Fact]
    [Trait("TestCase", "TC-1102")]
    public void Premium_quote_adds_vat() =>
        Assert.Equal(174.00m, Pricing().Quote(Premium, 1).Total);

    [Fact]
    [Trait("TestCase", "TC-1105")]
    public void Zero_quantity_is_rejected()
    {
        var error = Assert.Throws<FacilityRuleException>(
            () => new PrintingValidator(new PrintingPolicy()).Validate(Premium, 0));
        Assert.Equal("Quantity must be at least 1.", error.Message);
    }
}
