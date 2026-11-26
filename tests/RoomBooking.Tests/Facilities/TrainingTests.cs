using RoomBooking.Core.Facilities.Training;
using RoomBooking.Core.Facilities.Shared;

using Xunit;

namespace RoomBooking.Tests.Facilities;

public sealed class TrainingTests
{
    private static TrainingPricing Pricing() => new(new TrainingPolicy(), new FacilityTax());

    private static readonly TrainingItem Premium = new()
    {
        Id = "TR3-B", Name = "Training Premium", UnitPrice = 219m, Capacity = 30,
    };

    [Fact]
    [Trait("TestCase", "TC-1332")]
    public void Premium_quote_adds_vat() =>
        Assert.Equal(314.81m, Pricing().Quote(Premium, 1).Total);

    [Fact]
    [Trait("TestCase", "TC-1335")]
    public void Zero_quantity_is_rejected()
    {
        var error = Assert.Throws<FacilityRuleException>(
            () => new TrainingValidator(new TrainingPolicy()).Validate(Premium, 0));
        Assert.Equal("Quantity must be 1 or more.", error.Message);
    }
}
