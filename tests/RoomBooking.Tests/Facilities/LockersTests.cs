using RoomBooking.Core.Facilities.Lockers;
using RoomBooking.Core.Facilities.Shared;
using RoomBooking.Core.Facilities.Security;
using Xunit;

namespace RoomBooking.Tests.Facilities;

public sealed class LockersTests
{
    private static LockersPricing Pricing() => new(new LockersPolicy(), new FacilityTax(), new SecurityPolicy());

    private static readonly LockersItem Premium = new()
    {
        Id = "LOC-B", Name = "Lockers Premium", UnitPrice = 141m, Capacity = 30,
    };

    [Fact]
    [Trait("TestCase", "TC-1072")]
    public void Premium_quote_adds_vat() =>
        Assert.Equal(223.33m, Pricing().Quote(Premium, 1).Total);

    [Fact]
    [Trait("TestCase", "TC-1075")]
    public void Zero_quantity_is_rejected()
    {
        var error = Assert.Throws<FacilityRuleException>(
            () => new LockersValidator(new LockersPolicy()).Validate(Premium, 0));
        Assert.Equal("Quantity must be at least 1.", error.Message);
    }
}
