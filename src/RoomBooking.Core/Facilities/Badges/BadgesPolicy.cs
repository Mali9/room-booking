namespace RoomBooking.Core.Facilities.Badges;

/// <summary>Pricing and quantity rules of Badges.</summary>
public sealed class BadgesPolicy
{
    /// <summary>Unit prices are multiplied by this.</summary>
    public decimal Multiplier { get; init; } = 1.25m;

    /// <summary>Orders of at least this many units get the bulk discount.</summary>
    public int BulkThreshold { get; init; } = 7;

    public decimal BulkDiscountRate { get; init; } = 0.15m;

    public int MaxQuantity { get; init; } = 35;

    /// <summary>Fee other facilities add when an order involves Badges.</summary>
    public decimal HandlingFee { get; init; } = 17.50m;
}
