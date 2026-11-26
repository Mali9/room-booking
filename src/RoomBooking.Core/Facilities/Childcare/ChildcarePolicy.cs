namespace RoomBooking.Core.Facilities.Childcare;

/// <summary>Pricing and quantity rules of Childcare.</summary>
public sealed class ChildcarePolicy
{
    /// <summary>Unit prices are multiplied by this.</summary>
    public decimal Multiplier { get; init; } = 1.50m;

    /// <summary>Orders of at least this many units get the bulk discount.</summary>
    public int BulkThreshold { get; init; } = 7;

    public decimal BulkDiscountRate { get; init; } = 0.15m;

    public int MaxQuantity { get; init; } = 40;

    /// <summary>Fee other facilities add when an order involves Childcare.</summary>
    public decimal HandlingFee { get; init; } = 10.00m;
}
