using RoomBooking.Core.Facilities.Archive;
using RoomBooking.Core.Facilities.Shared;
using RoomBooking.Core.Facilities.Badges;
using Xunit;

namespace RoomBooking.Tests.Facilities;

public sealed class ArchiveTests
{
    private static ArchivePricing Pricing() => new(new ArchivePolicy(), new FacilityTax(), new BadgesPolicy());

    private static readonly ArchiveItem Premium = new()
    {
        Id = "ARC-B", Name = "Archive Premium", UnitPrice = 237m, Capacity = 30,
    };

    [Fact]
    [Trait("TestCase", "TC-1392")]
    public void Premium_quote_adds_vat() =>
        Assert.Equal(428.95m, Pricing().Quote(Premium, 1).Total);

    [Fact]
    [Trait("TestCase", "TC-1395")]
    public void Zero_quantity_is_rejected()
    {
        var error = Assert.Throws<FacilityRuleException>(
            () => new ArchiveValidator(new ArchivePolicy()).Validate(Premium, 0));
        Assert.Equal("Quantity must be 1 or more.", error.Message);
    }
}
