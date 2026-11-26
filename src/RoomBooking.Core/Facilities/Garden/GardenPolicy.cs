namespace RoomBooking.Core.Facilities.Garden;

/// <summary>Pricing and quantity rules of Garden.</summary>
public sealed class GardenPolicy
{
    /// <summary>Unit prices are multiplied by this.</summary>
    public decimal Multiplier { get; init; } = 1.25m;

    /// <summary>Orders of at least this many units get the bulk discount.</summary>
    public int BulkThreshold { get; init; } = 8;

    public decimal BulkDiscountRate { get; init; } = 0.15m;

    public int MaxQuantity { get; init; } = 35;

    /// <summary>Fee other facilities add when an order involves Garden.</summary>
    public decimal HandlingFee { get; init; } = 15.00m;
}
