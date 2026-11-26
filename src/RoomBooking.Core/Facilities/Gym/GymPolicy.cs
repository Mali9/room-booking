namespace RoomBooking.Core.Facilities.Gym;

/// <summary>Pricing and quantity rules of Gym.</summary>
public sealed class GymPolicy
{
    /// <summary>Unit prices are multiplied by this.</summary>
    public decimal Multiplier { get; init; } = 1.25m;

    /// <summary>Orders of at least this many units get the bulk discount.</summary>
    public int BulkThreshold { get; init; } = 6;

    public decimal BulkDiscountRate { get; init; } = 0.10m;

    public int MaxQuantity { get; init; } = 35;

    /// <summary>Fee other facilities add when an order involves Gym.</summary>
    public decimal HandlingFee { get; init; } = 25.00m;
}
