namespace RoomBooking.Core.Facilities.Kitchen;

/// <summary>Pricing and quantity rules of Kitchen.</summary>
public sealed class KitchenPolicy
{
    /// <summary>Unit prices are multiplied by this.</summary>
    public decimal Multiplier { get; init; } = 1.20m;

    /// <summary>Orders of at least this many units get the bulk discount.</summary>
    public int BulkThreshold { get; init; } = 7;

    public decimal BulkDiscountRate { get; init; } = 0.10m;

    public int MaxQuantity { get; init; } = 30;

    /// <summary>Fee other facilities add when an order involves Kitchen.</summary>
    public decimal HandlingFee { get; init; } = 12.50m;
}
