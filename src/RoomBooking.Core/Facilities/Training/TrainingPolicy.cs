namespace RoomBooking.Core.Facilities.Training;

/// <summary>Pricing and quantity rules of Training.</summary>
public sealed class TrainingPolicy
{
    /// <summary>Unit prices are multiplied by this.</summary>
    public decimal Multiplier { get; init; } = 1.25m;

    /// <summary>Orders of at least this many units get the bulk discount.</summary>
    public int BulkThreshold { get; init; } = 6;

    public decimal BulkDiscountRate { get; init; } = 0.05m;

    public int MaxQuantity { get; init; } = 35;

    /// <summary>Fee other facilities add when an order involves Training.</summary>
    public decimal HandlingFee { get; init; } = 22.50m;
}
