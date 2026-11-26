namespace RoomBooking.Core.Facilities.Printing;

/// <summary>Pricing and quantity rules of Printing.</summary>
public sealed class PrintingPolicy
{
    /// <summary>Unit prices are multiplied by this.</summary>
    public decimal Multiplier { get; init; } = 1.00m;

    /// <summary>Orders of at least this many units get the bulk discount.</summary>
    public int BulkThreshold { get; init; } = 7;

    public decimal BulkDiscountRate { get; init; } = 0.10m;

    public int MaxQuantity { get; init; } = 20;

    /// <summary>Fee other facilities add when an order involves Printing.</summary>
    public decimal HandlingFee { get; init; } = 17.50m;
}
