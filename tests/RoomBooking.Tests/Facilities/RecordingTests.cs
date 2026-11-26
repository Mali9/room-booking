using RoomBooking.Core.Facilities.Recording;
using RoomBooking.Core.Facilities.Shared;

using Xunit;

namespace RoomBooking.Tests.Facilities;

public sealed class RecordingTests
{
    private static RecordingPricing Pricing() => new(new RecordingPolicy(), new FacilityTax());

    private static readonly RecordingItem Premium = new()
    {
        Id = "REC-B", Name = "Recording Premium", UnitPrice = 216m, Capacity = 30,
    };

    [Fact]
    [Trait("TestCase", "TC-1322")]
    public void Premium_quote_adds_vat() =>
        Assert.Equal(298.08m, Pricing().Quote(Premium, 1).Total);

    [Fact]
    [Trait("TestCase", "TC-1325")]
    public void Zero_quantity_is_rejected()
    {
        var error = Assert.Throws<FacilityRuleException>(
            () => new RecordingValidator(new RecordingPolicy()).Validate(Premium, 0));
        Assert.Equal("Quantity must be at least 1.", error.Message);
    }
}
