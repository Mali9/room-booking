using RoomBooking.Core.Facilities.Childcare;
using RoomBooking.Core.Facilities.Shared;

using Xunit;

namespace RoomBooking.Tests.Facilities;

public sealed class ChildcareTests
{
    private static ChildcarePricing Pricing() => new(new ChildcarePolicy(), new FacilityTax());

    private static readonly ChildcareItem Premium = new()
    {
        Id = "CHI-B", Name = "Childcare Premium", UnitPrice = 162m, Capacity = 30,
    };

    [Fact]
    [Trait("TestCase", "TC-1142")]
    public void Premium_quote_adds_vat() =>
        Assert.Equal(281.88m, Pricing().Quote(Premium, 1).Total);

    [Fact]
    [Trait("TestCase", "TC-1145")]
    public void Zero_quantity_is_rejected()
    {
        var error = Assert.Throws<FacilityRuleException>(
            () => new ChildcareValidator(new ChildcarePolicy()).Validate(Premium, 0));
        Assert.Equal("Quantity must be at least 1.", error.Message);
    }
}
