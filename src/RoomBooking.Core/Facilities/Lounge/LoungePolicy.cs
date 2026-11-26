namespace RoomBooking.Core.Facilities.Lounge;

/// <summary>Pricing and quantity rules of Lounge.</summary>
public sealed class LoungePolicy
{
    /// <summary>Unit prices are multiplied by this.</summary>
    public decimal Multiplier { get; init; } = 1.20m;

    /// <summary>Orders of at least this many units get the bulk discount.</summary>
    public int BulkThreshold { get; init; } = 5;

    public decimal BulkDiscountRate { get; init; } = 0.05m;

    public int MaxQuantity { get; init; } = 30;

    /// <summary>Fee other facilities add when an order involves Lounge.</summary>
    public decimal HandlingFee { get; init; } = 22.50m;
}
