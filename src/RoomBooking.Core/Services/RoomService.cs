using RoomBooking.Core.Abstractions;
using RoomBooking.Core.Domain;

namespace RoomBooking.Core.Services;

/// <summary>Room catalogue and administration (BRD BR-01 and BR-02).</summary>
public sealed class RoomService(
    IRoomRepository rooms,
    IBookingRepository bookings,
    IClock clock)
{
    public IReadOnlyList<Room> GetCatalogue(bool includeInactive) =>
        rooms
            .GetAll()
            .Where(room => includeInactive || room.IsActive)
            .OrderBy(room => room.Id, StringComparer.Ordinal)
            .ToList();

    public Room Get(string roomId) =>
        rooms.GetById(roomId)
        ?? throw new BusinessRuleException(ErrorCodes.NotFound, "Room not found.");

    public Room Create(string name, int capacity, decimal hourlyRate)
    {
        var trimmed = Validate(name, capacity, hourlyRate, currentRoomId: null);
        var room = new Room
        {
            Id = rooms.NextId(),
            Name = trimmed,
            Capacity = capacity,
            HourlyRate = hourlyRate,
            IsActive = true,
        };
        rooms.Add(room);
        return room;
    }

    public Room Update(string roomId, string name, int capacity, decimal hourlyRate)
    {
        var room = Get(roomId);
        room.Name = Validate(name, capacity, hourlyRate, roomId);
        room.Capacity = capacity;
        room.HourlyRate = hourlyRate;
        rooms.Update(room);
        return room;
    }

    public Room SetActive(string roomId, bool isActive)
    {
        var room = Get(roomId);
        if (!isActive && HasUpcomingBookings(roomId))
        {
            throw new BusinessRuleException(
                ErrorCodes.RoomHasUpcomingBookings,
                $"Room {room.Name} has upcoming bookings and cannot be deactivated.");
        }

        room.IsActive = isActive;
        rooms.Update(room);
        return room;
    }

    private bool HasUpcomingBookings(string roomId) =>
        bookings.GetConfirmedForRoom(roomId).Any(booking => booking.Start > clock.Now);

    private string Validate(string name, int capacity, decimal hourlyRate, string? currentRoomId)
    {
        var trimmed = (name ?? string.Empty).Trim();
        if (trimmed.Length < 3 || trimmed.Length > 50)
        {
            throw new BusinessRuleException(
                ErrorCodes.InvalidRoom,
                "Room name must be between 3 and 50 characters.");
        }

        if (capacity < 1 || capacity > 50)
        {
            throw new BusinessRuleException(
                ErrorCodes.InvalidRoom,
                "Capacity must be between 1 and 50.");
        }

        if (hourlyRate < 10m || hourlyRate > 5000m)
        {
            throw new BusinessRuleException(
                ErrorCodes.InvalidRoom,
                "Hourly rate must be between 10 and 5,000 SAR.");
        }

        var duplicate = rooms
            .GetAll()
            .Any(room => room.Id != currentRoomId
                && string.Equals(room.Name, trimmed, StringComparison.OrdinalIgnoreCase));
        if (duplicate)
        {
            throw new BusinessRuleException(
                ErrorCodes.DuplicateRoomName,
                $"A room named '{trimmed}' already exists.");
        }

        return trimmed;
    }
}
