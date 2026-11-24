using RoomBooking.Core.Abstractions;

namespace RoomBooking.Infrastructure;

public sealed class SystemClock : IClock
{
    public DateTime Now => DateTime.Now;
}

/// <summary>A frozen clock so test runs see the same "now" (configured by Clock:FixedNow).</summary>
public sealed class FixedClock(DateTime now) : IClock
{
    public DateTime Now { get; } = now;
}
