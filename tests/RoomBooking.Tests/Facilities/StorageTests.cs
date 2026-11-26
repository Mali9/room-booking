using RoomBooking.Core.Facilities.Storage;
using RoomBooking.Core.Facilities.Shared;

using Xunit;

namespace RoomBooking.Tests.Facilities;

public sealed class StorageTests
{
    private static StoragePricing Pricing() => new(new StoragePolicy(), new FacilityTax());

    private static readonly StorageItem Premium = new()
    {
        Id = "STO-B", Name = "Storage Premium", UnitPrice = 171m, Capacity = 30,
    };

    [Fact]
    [Trait("TestCase", "TC-1172")]
    public void Premium_quote_adds_vat() =>
        Assert.Equal(235.98m, Pricing().Quote(Premium, 1).Total);

    [Fact]
    [Trait("TestCase", "TC-1175")]
    public void Zero_quantity_is_rejected()
    {
        var error = Assert.Throws<FacilityRuleException>(
            () => new StorageValidator(new StoragePolicy()).Validate(Premium, 0));
        Assert.Equal("Quantity must be 1 or more.", error.Message);
    }
}
