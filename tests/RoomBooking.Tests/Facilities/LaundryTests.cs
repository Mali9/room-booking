using RoomBooking.Core.Facilities.Laundry;
using RoomBooking.Core.Facilities.Shared;
using RoomBooking.Core.Facilities.Childcare;
using Xunit;

namespace RoomBooking.Tests.Facilities;

public sealed class LaundryTests
{
    private static LaundryPricing Pricing() => new(new LaundryPolicy(), new FacilityTax(), new ChildcarePolicy());

    private static readonly LaundryItem Premium = new()
    {
        Id = "LAU-B", Name = "Laundry Premium", UnitPrice = 165m, Capacity = 30,
    };

    [Fact]
    [Trait("TestCase", "TC-1152")]
    public void Premium_quote_adds_vat() =>
        Assert.Equal(201.25m, Pricing().Quote(Premium, 1).Total);

    [Fact]
    [Trait("TestCase", "TC-1155")]
    public void Zero_quantity_is_rejected()
    {
        var error = Assert.Throws<FacilityRuleException>(
            () => new LaundryValidator(new LaundryPolicy()).Validate(Premium, 0));
        Assert.Equal("Quantity must be at least 1.", error.Message);
    }
}
