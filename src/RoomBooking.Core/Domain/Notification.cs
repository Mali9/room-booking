namespace RoomBooking.Core.Domain;

public enum NotificationType
{
    BookingConfirmed,
    BookingCancelled,
    BookingRescheduled,
}

public sealed class Notification
{
    public int Id { get; init; }
    public required string UserId { get; init; }
    public NotificationType Type { get; init; }
    public required string Message { get; init; }
    public DateTime CreatedAt { get; init; }
    public bool IsRead { get; set; }
}
