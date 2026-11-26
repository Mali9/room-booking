namespace RoomBooking.Core.Facilities.Translation;

/// <summary>Pricing and quantity rules of Translation.</summary>
public sealed class TranslationPolicy
{
    /// <summary>Unit prices are multiplied by this.</summary>
    public decimal Multiplier { get; init; } = 1.20m;

    /// <summary>Orders of at least this many units get the bulk discount.</summary>
    public int BulkThreshold { get; init; } = 8;

    public decimal BulkDiscountRate { get; init; } = 0.05m;

    public int MaxQuantity { get; init; } = 30;

    /// <summary>Fee other facilities add when an order involves Translation.</summary>
    public decimal HandlingFee { get; init; } = 25.00m;
}
