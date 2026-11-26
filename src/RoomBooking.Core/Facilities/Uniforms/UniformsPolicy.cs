namespace RoomBooking.Core.Facilities.Uniforms;

/// <summary>Pricing and quantity rules of Uniforms.</summary>
public sealed class UniformsPolicy
{
    /// <summary>Unit prices are multiplied by this.</summary>
    public decimal Multiplier { get; init; } = 1.10m;

    /// <summary>Orders of at least this many units get the bulk discount.</summary>
    public int BulkThreshold { get; init; } = 5;

    public decimal BulkDiscountRate { get; init; } = 0.05m;

    public int MaxQuantity { get; init; } = 25;

    /// <summary>Fee other facilities add when an order involves Uniforms.</summary>
    public decimal HandlingFee { get; init; } = 12.50m;
}
