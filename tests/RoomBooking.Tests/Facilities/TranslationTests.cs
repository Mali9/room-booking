using RoomBooking.Core.Facilities.Translation;
using RoomBooking.Core.Facilities.Shared;
using RoomBooking.Core.Facilities.Courier;
using Xunit;

namespace RoomBooking.Tests.Facilities;

public sealed class TranslationTests
{
    private static TranslationPricing Pricing() => new(new TranslationPolicy(), new FacilityTax(), new CourierPolicy());

    private static readonly TranslationItem Premium = new()
    {
        Id = "TRA-B", Name = "Translation Premium", UnitPrice = 201m, Capacity = 30,
    };

    [Fact]
    [Trait("TestCase", "TC-1272")]
    public void Premium_quote_adds_vat() =>
        Assert.Equal(303.26m, Pricing().Quote(Premium, 1).Total);

    [Fact]
    [Trait("TestCase", "TC-1275")]
    public void Zero_quantity_is_rejected()
    {
        var error = Assert.Throws<FacilityRuleException>(
            () => new TranslationValidator(new TranslationPolicy()).Validate(Premium, 0));
        Assert.Equal("Quantity must be at least 1.", error.Message);
    }
}
