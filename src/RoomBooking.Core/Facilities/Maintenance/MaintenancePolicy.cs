namespace RoomBooking.Core.Facilities.Maintenance;

/// <summary>Pricing and quantity rules of Maintenance.</summary>
public sealed class MaintenancePolicy
{
    /// <summary>Unit prices are multiplied by this.</summary>
    public decimal Multiplier { get; init; } = 1.00m;

    /// <summary>Orders of at least this many units get the bulk discount.</summary>
    public int BulkThreshold { get; init; } = 6;

    public decimal BulkDiscountRate { get; init; } = 0.15m;

    public int MaxQuantity { get; init; } = 20;

    /// <summary>Fee other facilities add when an order involves Maintenance.</summary>
    public decimal HandlingFee { get; init; } = 22.50m;
}
