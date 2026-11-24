namespace RoomBooking.Core;

/// <summary>A business rule rejected the request. The message is safe to show to users.</summary>
public sealed class BusinessRuleException(string code, string message) : Exception(message)
{
    public string Code { get; } = code;
}

public static class ErrorCodes
{
    public const string NotFound = "NOT_FOUND";
    public const string InvalidRoom = "INVALID_ROOM";
    public const string DuplicateRoomName = "DUPLICATE_ROOM_NAME";
    public const string RoomHasUpcomingBookings = "ROOM_HAS_UPCOMING_BOOKINGS";
    public const string RoomInactive = "ROOM_INACTIVE";
    public const string CapacityExceeded = "CAPACITY_EXCEEDED";
    public const string InvalidDuration = "INVALID_DURATION";
    public const string InvalidStartTime = "INVALID_START_TIME";
    public const string StartInPast = "START_IN_PAST";
    public const string OutsideOpeningHours = "OUTSIDE_OPENING_HOURS";
    public const string SlotUnavailable = "SLOT_UNAVAILABLE";
    public const string InvalidStatus = "INVALID_STATUS";
}
