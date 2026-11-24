namespace RoomBooking.Core.Services;

/// <summary>Business pricing constants (BRD BR-08 to BR-11).</summary>
public sealed class PricingPolicy
{
    /// <summary>Peak hours cost the hourly rate times this multiplier.</summary>
    public decimal PeakMultiplier { get; init; } = 1.5m;

    public TimeSpan PeakStart { get; init; } = TimeSpan.FromHours(9);
    public TimeSpan PeakEnd { get; init; } = TimeSpan.FromHours(17);

    /// <summary>Working days. Friday and Saturday are off-peak all day.</summary>
    public IReadOnlySet<DayOfWeek> PeakDays { get; init; } = new HashSet<DayOfWeek>
    {
        DayOfWeek.Sunday,
        DayOfWeek.Monday,
        DayOfWeek.Tuesday,
        DayOfWeek.Wednesday,
        DayOfWeek.Thursday,
    };

    public int LongBookingHours { get; init; } = 4;
    public decimal LongBookingDiscountRate { get; init; } = 0.10m;
    public decimal VatRate { get; init; } = 0.15m;
}
