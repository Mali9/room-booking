namespace RoomBooking.Core.Facilities.Cleaning;

/// <summary>Pricing and quantity rules of Cleaning.</summary>
public sealed class CleaningPolicy
{
    /// <summary>Unit prices are multiplied by this.</summary>
    public decimal Multiplier { get; init; } = 1.50m;

    /// <summary>Orders of at least this many units get the bulk discount.</summary>
    public int BulkThreshold { get; init; } = 5;

    public decimal BulkDiscountRate { get; init; } = 0.10m;

    public int MaxQuantity { get; init; } = 40;

    /// <summary>Fee other facilities add when an order involves Cleaning.</summary>
    public decimal HandlingFee { get; init; } = 20.00m;
}
