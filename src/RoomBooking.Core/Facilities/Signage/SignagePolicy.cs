namespace RoomBooking.Core.Facilities.Signage;

/// <summary>Pricing and quantity rules of Signage.</summary>
public sealed class SignagePolicy
{
    /// <summary>Unit prices are multiplied by this.</summary>
    public decimal Multiplier { get; init; } = 1.00m;

    /// <summary>Orders of at least this many units get the bulk discount.</summary>
    public int BulkThreshold { get; init; } = 7;

    public decimal BulkDiscountRate { get; init; } = 0.05m;

    public int MaxQuantity { get; init; } = 20;

    /// <summary>Fee other facilities add when an order involves Signage.</summary>
    public decimal HandlingFee { get; init; } = 15.00m;
}
