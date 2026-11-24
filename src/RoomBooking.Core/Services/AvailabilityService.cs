using RoomBooking.Core.Abstractions;
using RoomBooking.Core.Domain;

namespace RoomBooking.Core.Services;

/// <summary>Booking slot rules and room availability (BRD BR-03 to BR-07).</summary>
public sealed class AvailabilityService(
    IRoomRepository rooms,
    IBookingRepository bookings,
    IClock clock)
{
    public const int MinHours = 1;
    public const int MaxHours = 8;
    public static readonly TimeSpan OpeningTime = TimeSpan.FromHours(8);
    public static readonly TimeSpan ClosingTime = TimeSpan.FromHours(22);

    public void ValidateSlot(DateTime start, int hours)
    {
        if (hours < MinHours || hours > MaxHours)
        {
            throw new BusinessRuleException(
                ErrorCodes.InvalidDuration,
                $"Duration must be between {MinHours} and {MaxHours} hours.");
        }

        if (start.Minute != 0 || start.Second != 0)
        {
            throw new BusinessRuleException(
                ErrorCodes.InvalidStartTime,
                "Bookings must start on the hour.");
        }

        if (start <= clock.Now)
        {
            throw new BusinessRuleException(
                ErrorCodes.StartInPast,
                "The start time must be in the future.");
        }

        var end = start.AddHours(hours);
        if (start.TimeOfDay < OpeningTime || end.Date != start.Date || end.TimeOfDay > ClosingTime)
        {
            throw new BusinessRuleException(
                ErrorCodes.OutsideOpeningHours,
                "Bookings must start at 08:00 or later and end by 22:00.");
        }
    }

    public bool IsRoomFree(string roomId, DateTime start, int hours, string? ignoreBookingId = null)
    {
        var end = start.AddHours(hours);
        return bookings
            .GetConfirmedForRoom(roomId)
            .Where(booking => booking.Id != ignoreBookingId)
            .All(booking => !booking.Overlaps(start, end));
    }

    public IReadOnlyList<Room> FindAvailableRooms(DateTime start, int hours, int attendees)
    {
        ValidateSlot(start, hours);
        return rooms
            .GetAll()
            .Where(room => room.IsActive)
            .Where(room => room.Capacity >= attendees)
            .Where(room => IsRoomFree(room.Id, start, hours))
            .OrderBy(room => room.HourlyRate)
            .ThenBy(room => room.Name, StringComparer.Ordinal)
            .ToList();
    }

    public void EnsureBookable(
        Room room,
        DateTime start,
        int hours,
        int attendees,
        string? ignoreBookingId = null)
    {
        ValidateSlot(start, hours);
        if (!room.IsActive)
        {
            throw new BusinessRuleException(
                ErrorCodes.RoomInactive,
                $"Room {room.Name} is not available for booking.");
        }

        if (attendees < 1 || attendees > room.Capacity)
        {
            throw new BusinessRuleException(
                ErrorCodes.CapacityExceeded,
                $"Room {room.Name} holds at most {room.Capacity} people.");
        }

        if (!IsRoomFree(room.Id, start, hours, ignoreBookingId))
        {
            throw new BusinessRuleException(
                ErrorCodes.SlotUnavailable,
                $"Room {room.Name} is already booked at that time.");
        }
    }
}
