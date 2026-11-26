namespace RoomBooking.Core.Facilities.Keys;

/// <summary>Pricing and quantity rules of Keys.</summary>
public sealed class KeysPolicy
{
    /// <summary>Unit prices are multiplied by this.</summary>
    public decimal Multiplier { get; init; } = 1.20m;

    /// <summary>Orders of at least this many units get the bulk discount.</summary>
    public int BulkThreshold { get; init; } = 6;

    public decimal BulkDiscountRate { get; init; } = 0.10m;

    public int MaxQuantity { get; init; } = 30;

    /// <summary>Fee other facilities add when an order involves Keys.</summary>
    public decimal HandlingFee { get; init; } = 15.00m;
}
