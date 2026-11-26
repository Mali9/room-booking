using RoomBooking.Core.Facilities.Library;
using RoomBooking.Core.Facilities.Shared;
using RoomBooking.Core.Facilities.Wellness;
using Xunit;

namespace RoomBooking.Tests.Facilities;

public sealed class LibraryTests
{
    private static LibraryPricing Pricing() => new(new LibraryPolicy(), new FacilityTax(), new WellnessPolicy());

    private static readonly LibraryItem Premium = new()
    {
        Id = "LIB-B", Name = "Library Premium", UnitPrice = 177m, Capacity = 30,
    };

    [Fact]
    [Trait("TestCase", "TC-1192")]
    public void Premium_quote_adds_vat() =>
        Assert.Equal(331.18m, Pricing().Quote(Premium, 1).Total);

    [Fact]
    [Trait("TestCase", "TC-1195")]
    public void Zero_quantity_is_rejected()
    {
        var error = Assert.Throws<FacilityRuleException>(
            () => new LibraryValidator(new LibraryPolicy()).Validate(Premium, 0));
        Assert.Equal("Quantity must be at least 1.", error.Message);
    }
}
