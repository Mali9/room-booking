namespace RoomBooking.Core.Facilities.Parking;

/// <summary>Pricing and quantity rules of Parking.</summary>
public sealed class ParkingPolicy
{
    /// <summary>Unit prices are multiplied by this.</summary>
    public decimal Multiplier { get; init; } = 1.30m;

    /// <summary>Orders of at least this many units get the bulk discount.</summary>
    public int BulkThreshold { get; init; } = 6;

    public decimal BulkDiscountRate { get; init; } = 0.10m;

    public int MaxQuantity { get; init; } = 25;

    /// <summary>Fee other facilities add when an order involves Parking.</summary>
    public decimal HandlingFee { get; init; } = 12.50m;
}
