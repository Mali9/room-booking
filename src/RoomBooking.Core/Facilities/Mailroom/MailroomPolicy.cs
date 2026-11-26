namespace RoomBooking.Core.Facilities.Mailroom;

/// <summary>Pricing and quantity rules of Mailroom.</summary>
public sealed class MailroomPolicy
{
    /// <summary>Unit prices are multiplied by this.</summary>
    public decimal Multiplier { get; init; } = 1.10m;

    /// <summary>Orders of at least this many units get the bulk discount.</summary>
    public int BulkThreshold { get; init; } = 5;

    public decimal BulkDiscountRate { get; init; } = 0.10m;

    public int MaxQuantity { get; init; } = 25;

    /// <summary>Fee other facilities add when an order involves Mailroom.</summary>
    public decimal HandlingFee { get; init; } = 15.00m;
}
