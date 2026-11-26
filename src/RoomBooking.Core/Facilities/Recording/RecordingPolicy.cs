namespace RoomBooking.Core.Facilities.Recording;

/// <summary>Pricing and quantity rules of Recording.</summary>
public sealed class RecordingPolicy
{
    /// <summary>Unit prices are multiplied by this.</summary>
    public decimal Multiplier { get; init; } = 1.20m;

    /// <summary>Orders of at least this many units get the bulk discount.</summary>
    public int BulkThreshold { get; init; } = 5;

    public decimal BulkDiscountRate { get; init; } = 0.15m;

    public int MaxQuantity { get; init; } = 30;

    /// <summary>Fee other facilities add when an order involves Recording.</summary>
    public decimal HandlingFee { get; init; } = 20.00m;
}
