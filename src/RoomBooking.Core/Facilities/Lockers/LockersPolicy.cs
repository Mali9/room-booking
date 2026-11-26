namespace RoomBooking.Core.Facilities.Lockers;

/// <summary>Pricing and quantity rules of Lockers.</summary>
public sealed class LockersPolicy
{
    /// <summary>Unit prices are multiplied by this.</summary>
    public decimal Multiplier { get; init; } = 1.20m;

    /// <summary>Orders of at least this many units get the bulk discount.</summary>
    public int BulkThreshold { get; init; } = 8;

    public decimal BulkDiscountRate { get; init; } = 0.10m;

    public int MaxQuantity { get; init; } = 30;

    /// <summary>Fee other facilities add when an order involves Lockers.</summary>
    public decimal HandlingFee { get; init; } = 10.00m;
}
