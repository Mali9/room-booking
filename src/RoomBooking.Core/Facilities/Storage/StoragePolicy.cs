namespace RoomBooking.Core.Facilities.Storage;

/// <summary>Pricing and quantity rules of Storage.</summary>
public sealed class StoragePolicy
{
    /// <summary>Unit prices are multiplied by this.</summary>
    public decimal Multiplier { get; init; } = 1.20m;

    /// <summary>Orders of at least this many units get the bulk discount.</summary>
    public int BulkThreshold { get; init; } = 6;

    public decimal BulkDiscountRate { get; init; } = 0.15m;

    public int MaxQuantity { get; init; } = 30;

    /// <summary>Fee other facilities add when an order involves Storage.</summary>
    public decimal HandlingFee { get; init; } = 17.50m;
}
