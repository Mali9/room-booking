namespace RoomBooking.Core.Facilities.Wellness;

/// <summary>Pricing and quantity rules of Wellness.</summary>
public sealed class WellnessPolicy
{
    /// <summary>Unit prices are multiplied by this.</summary>
    public decimal Multiplier { get; init; } = 1.25m;

    /// <summary>Orders of at least this many units get the bulk discount.</summary>
    public int BulkThreshold { get; init; } = 7;

    public decimal BulkDiscountRate { get; init; } = 0.05m;

    public int MaxQuantity { get; init; } = 35;

    /// <summary>Fee other facilities add when an order involves Wellness.</summary>
    public decimal HandlingFee { get; init; } = 20.00m;
}
