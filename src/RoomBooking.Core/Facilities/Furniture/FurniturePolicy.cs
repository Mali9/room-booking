namespace RoomBooking.Core.Facilities.Furniture;

/// <summary>Pricing and quantity rules of Furniture.</summary>
public sealed class FurniturePolicy
{
    /// <summary>Unit prices are multiplied by this.</summary>
    public decimal Multiplier { get; init; } = 1.50m;

    /// <summary>Orders of at least this many units get the bulk discount.</summary>
    public int BulkThreshold { get; init; } = 6;

    public decimal BulkDiscountRate { get; init; } = 0.15m;

    public int MaxQuantity { get; init; } = 40;

    /// <summary>Fee other facilities add when an order involves Furniture.</summary>
    public decimal HandlingFee { get; init; } = 12.50m;
}
