namespace RoomBooking.Core.Facilities.ItSupport;

/// <summary>Pricing and quantity rules of ItSupport.</summary>
public sealed class ItSupportPolicy
{
    /// <summary>Unit prices are multiplied by this.</summary>
    public decimal Multiplier { get; init; } = 1.25m;

    /// <summary>Orders of at least this many units get the bulk discount.</summary>
    public int BulkThreshold { get; init; } = 5;

    public decimal BulkDiscountRate { get; init; } = 0.10m;

    public int MaxQuantity { get; init; } = 35;

    /// <summary>Fee other facilities add when an order involves ItSupport.</summary>
    public decimal HandlingFee { get; init; } = 10.00m;
}
