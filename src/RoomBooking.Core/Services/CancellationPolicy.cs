using RoomBooking.Core.Domain;

namespace RoomBooking.Core.Services;

/// <summary>Cancellation fee rules (BRD BR-13 and BR-14).</summary>
public sealed class CancellationPolicy
{
    /// <summary>Cancelling at least this many hours before the start is free.</summary>
    public int FreeCancellationHours { get; init; } = 24;

    /// <summary>Share of the booking price charged for a late cancellation.</summary>
    public decimal LateFeeRate { get; init; } = 0.5m;

    public decimal CalculateFee(Booking booking, DateTime now, bool waived)
    {
        if (waived)
        {
            return 0m;
        }

        var noticeHours = (booking.Start - now).TotalHours;
        if (noticeHours >= FreeCancellationHours)
        {
            return 0m;
        }

        return Math.Round(booking.Price * LateFeeRate, 2, MidpointRounding.AwayFromZero);
    }
}
