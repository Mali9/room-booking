namespace RoomBooking.Core.Facilities.Studio;

/// <summary>Pricing and quantity rules of Studio.</summary>
public sealed class StudioPolicy
{
    /// <summary>Unit prices are multiplied by this.</summary>
    public decimal Multiplier { get; init; } = 1.10m;

    /// <summary>Orders of at least this many units get the bulk discount.</summary>
    public int BulkThreshold { get; init; } = 6;

    public decimal BulkDiscountRate { get; init; } = 0.05m;

    public int MaxQuantity { get; init; } = 25;

    /// <summary>Fee other facilities add when an order involves Studio.</summary>
    public decimal HandlingFee { get; init; } = 10.00m;
}
