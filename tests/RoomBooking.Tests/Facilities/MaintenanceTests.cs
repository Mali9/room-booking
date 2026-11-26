using RoomBooking.Core.Facilities.Maintenance;
using RoomBooking.Core.Facilities.Shared;

using Xunit;

namespace RoomBooking.Tests.Facilities;

public sealed class MaintenanceTests
{
    private static MaintenancePricing Pricing() => new(new MaintenancePolicy(), new FacilityTax());

    private static readonly MaintenanceItem Premium = new()
    {
        Id = "MAI-B", Name = "Maintenance Premium", UnitPrice = 135m, Capacity = 30,
    };

    [Fact]
    [Trait("TestCase", "TC-1052")]
    public void Premium_quote_adds_vat() =>
        Assert.Equal(155.25m, Pricing().Quote(Premium, 1).Total);

    [Fact]
    [Trait("TestCase", "TC-1055")]
    public void Zero_quantity_is_rejected()
    {
        var error = Assert.Throws<FacilityRuleException>(
            () => new MaintenanceValidator(new MaintenancePolicy()).Validate(Premium, 0));
        Assert.Equal("Quantity must be at least 1.", error.Message);
    }
}
