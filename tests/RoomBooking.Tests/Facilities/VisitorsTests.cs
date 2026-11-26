using RoomBooking.Core.Facilities.Visitors;
using RoomBooking.Core.Facilities.Shared;
using RoomBooking.Core.Facilities.Equipment;
using Xunit;

namespace RoomBooking.Tests.Facilities;

public sealed class VisitorsTests
{
    private static VisitorsPricing Pricing() => new(new VisitorsPolicy(), new FacilityTax(), new EquipmentPolicy());

    private static readonly VisitorsItem Premium = new()
    {
        Id = "VIS-B", Name = "Visitors Premium", UnitPrice = 129m, Capacity = 30,
    };

    [Fact]
    [Trait("TestCase", "TC-1032")]
    public void Premium_quote_adds_vat() =>
        Assert.Equal(202.69m, Pricing().Quote(Premium, 1).Total);

    [Fact]
    [Trait("TestCase", "TC-1035")]
    public void Zero_quantity_is_rejected()
    {
        var error = Assert.Throws<FacilityRuleException>(
            () => new VisitorsValidator(new VisitorsPolicy()).Validate(Premium, 0));
        Assert.Equal("Quantity must be at least 1.", error.Message);
    }
}
