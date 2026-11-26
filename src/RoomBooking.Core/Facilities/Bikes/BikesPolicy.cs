namespace RoomBooking.Core.Facilities.Bikes;

/// <summary>Pricing and quantity rules of Bikes.</summary>
public sealed class BikesPolicy
{
    /// <summary>Unit prices are multiplied by this.</summary>
    public decimal Multiplier { get; init; } = 1.00m;

    /// <summary>Orders of at least this many units get the bulk discount.</summary>
    public int BulkThreshold { get; init; } = 6;

    public decimal BulkDiscountRate { get; init; } = 0.10m;

    public int MaxQuantity { get; init; } = 20;

    /// <summary>Fee other facilities add when an order involves Bikes.</summary>
    public decimal HandlingFee { get; init; } = 20.00m;
}
