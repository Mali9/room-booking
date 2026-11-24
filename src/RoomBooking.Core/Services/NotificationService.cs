using RoomBooking.Core.Abstractions;
using RoomBooking.Core.Domain;

namespace RoomBooking.Core.Services;

/// <summary>In-app notifications (BRD BR-18).</summary>
public sealed class NotificationService(INotificationRepository notifications, IClock clock)
{
    public void BookingConfirmed(Booking booking, Room room) =>
        notifications.Add(
            booking.CustomerId,
            NotificationType.BookingConfirmed,
            $"Booking {booking.Id} confirmed: {room.Name} on {Text.Date(booking.Start)} "
                + $"from {Text.Time(booking.Start)} to {Text.Time(booking.End)}.",
            clock.Now);

    public void BookingCancelled(Booking booking, decimal fee) =>
        notifications.Add(
            booking.CustomerId,
            NotificationType.BookingCancelled,
            $"Booking {booking.Id} cancelled. Cancellation fee: {Text.Money(fee)}.",
            clock.Now);

    public void BookingRescheduled(Booking booking, Room room, DateTime previousStart) =>
        notifications.Add(
            booking.CustomerId,
            NotificationType.BookingRescheduled,
            $"Booking {booking.Id} moved from {Text.DateTime(previousStart)} "
                + $"to {Text.DateTime(booking.Start)} ({room.Name}).",
            clock.Now);

    public IReadOnlyList<Notification> GetForUser(string userId) =>
        notifications
            .GetForUser(userId)
            .OrderByDescending(notification => notification.Id)
            .ToList();

    public int UnreadCount(string userId) =>
        notifications.GetForUser(userId).Count(notification => !notification.IsRead);

    public void MarkAllRead(string userId) => notifications.MarkAllRead(userId);
}
