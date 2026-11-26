using RoomBooking.Core.Facilities.Mailroom;
using RoomBooking.Core.Facilities.Shared;

using Xunit;

namespace RoomBooking.Tests.Facilities;

public sealed class MailroomTests
{
    private static MailroomPricing Pricing() => new(new MailroomPolicy(), new FacilityTax());

    private static readonly MailroomItem Premium = new()
    {
        Id = "MA6-B", Name = "Mailroom Premium", UnitPrice = 168m, Capacity = 30,
    };

    [Fact]
    [Trait("TestCase", "TC-1162")]
    public void Premium_quote_adds_vat() =>
        Assert.Equal(212.52m, Pricing().Quote(Premium, 1).Total);

    [Fact]
    [Trait("TestCase", "TC-1165")]
    public void Zero_quantity_is_rejected()
    {
        var error = Assert.Throws<FacilityRuleException>(
            () => new MailroomValidator(new MailroomPolicy()).Validate(Premium, 0));
        Assert.Equal("Quantity must be at least 1.", error.Message);
    }
}
