namespace RoomBooking.Core.Facilities.Catering;

/// <summary>Pricing and quantity rules of Catering.</summary>
public sealed class CateringPolicy
{
    /// <summary>Unit prices are multiplied by this.</summary>
    public decimal Multiplier { get; init; } = 1.00m;

    /// <summary>Orders of at least this many units get the bulk discount.</summary>
    public int BulkThreshold { get; init; } = 5;

    public decimal BulkDiscountRate { get; init; } = 0.05m;

    public int MaxQuantity { get; init; } = 20;

    /// <summary>Fee other facilities add when an order involves Catering.</summary>
    public decimal HandlingFee { get; init; } = 10.00m;
}
