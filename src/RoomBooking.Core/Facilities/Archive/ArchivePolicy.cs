namespace RoomBooking.Core.Facilities.Archive;

/// <summary>Pricing and quantity rules of Archive.</summary>
public sealed class ArchivePolicy
{
    /// <summary>Unit prices are multiplied by this.</summary>
    public decimal Multiplier { get; init; } = 1.50m;

    /// <summary>Orders of at least this many units get the bulk discount.</summary>
    public int BulkThreshold { get; init; } = 8;

    public decimal BulkDiscountRate { get; init; } = 0.05m;

    public int MaxQuantity { get; init; } = 40;

    /// <summary>Fee other facilities add when an order involves Archive.</summary>
    public decimal HandlingFee { get; init; } = 20.00m;
}
