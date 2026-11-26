namespace RoomBooking.Core.Facilities.Photography;

/// <summary>Pricing and quantity rules of Photography.</summary>
public sealed class PhotographyPolicy
{
    /// <summary>Unit prices are multiplied by this.</summary>
    public decimal Multiplier { get; init; } = 1.10m;

    /// <summary>Orders of at least this many units get the bulk discount.</summary>
    public int BulkThreshold { get; init; } = 8;

    public decimal BulkDiscountRate { get; init; } = 0.10m;

    public int MaxQuantity { get; init; } = 25;

    /// <summary>Fee other facilities add when an order involves Photography.</summary>
    public decimal HandlingFee { get; init; } = 17.50m;
}
