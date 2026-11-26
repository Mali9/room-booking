using RoomBooking.Core.Facilities.Events;
using RoomBooking.Core.Facilities.Shared;

using Xunit;

namespace RoomBooking.Tests.Facilities;

public sealed class EventsTests
{
    private static EventsPricing Pricing() => new(new EventsPolicy(), new FacilityTax());

    private static readonly EventsItem Premium = new()
    {
        Id = "EVE-B", Name = "Events Premium", UnitPrice = 147m, Capacity = 30,
    };

    [Fact]
    [Trait("TestCase", "TC-1092")]
    public void Premium_quote_adds_vat() =>
        Assert.Equal(255.78m, Pricing().Quote(Premium, 1).Total);

    [Fact]
    [Trait("TestCase", "TC-1095")]
    public void Zero_quantity_is_rejected()
    {
        var error = Assert.Throws<FacilityRuleException>(
            () => new EventsValidator(new EventsPolicy()).Validate(Premium, 0));
        Assert.Equal("Quantity must be at least 1.", error.Message);
    }
}
