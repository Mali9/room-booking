namespace RoomBooking.Core.Facilities.Shuttle;

/// <summary>Pricing and quantity rules of Shuttle.</summary>
public sealed class ShuttlePolicy
{
    /// <summary>Unit prices are multiplied by this.</summary>
    public decimal Multiplier { get; init; } = 1.10m;

    /// <summary>Orders of at least this many units get the bulk discount.</summary>
    public int BulkThreshold { get; init; } = 8;

    public decimal BulkDiscountRate { get; init; } = 0.15m;

    public int MaxQuantity { get; init; } = 25;

    /// <summary>Fee other facilities add when an order involves Shuttle.</summary>
    public decimal HandlingFee { get; init; } = 20.00m;
}
