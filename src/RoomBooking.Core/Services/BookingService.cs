using RoomBooking.Core.Abstractions;
using RoomBooking.Core.Domain;

namespace RoomBooking.Core.Services;

public sealed record BookingRequest(
    string CustomerId,
    string RoomId,
    DateTime Start,
    int Hours,
    int Attendees);

/// <summary>Booking lifecycle: create, cancel, reschedule (BRD BR-03 to BR-17).</summary>
public sealed class BookingService(
    IBookingRepository bookings,
    IRoomRepository rooms,
    AvailabilityService availability,
    PricingService pricing,
    CancellationPolicy cancellationPolicy,
    InvoiceService invoices,
    NotificationService notifications,
    IClock clock)
{
    public Booking Create(BookingRequest request)
    {
        var room = GetRoom(request.RoomId);
        availability.EnsureBookable(room, request.Start, request.Hours, request.Attendees);

        var quote = pricing.Quote(room, request.Start, request.Hours);
        var booking = new Booking
        {
            Id = bookings.NextId(),
            RoomId = room.Id,
            CustomerId = request.CustomerId,
            Start = request.Start,
            Hours = request.Hours,
            Attendees = request.Attendees,
            Status = BookingStatus.Confirmed,
            Price = quote.Total,
            CreatedAt = clock.Now,
        };
        bookings.Add(booking);
        invoices.IssueForBooking(booking, room, quote);
        notifications.BookingConfirmed(booking, room);
        return booking;
    }

    public decimal PreviewCancellationFee(string bookingId, string actorId, bool actorIsAdmin)
    {
        var booking = GetChangeable(bookingId, actorId, actorIsAdmin);
        return cancellationPolicy.CalculateFee(booking, clock.Now, waived: actorIsAdmin);
    }

    public Booking Cancel(string bookingId, string actorId, bool actorIsAdmin)
    {
        var booking = GetChangeable(bookingId, actorId, actorIsAdmin);
        var fee = cancellationPolicy.CalculateFee(booking, clock.Now, waived: actorIsAdmin);

        booking.Status = BookingStatus.Cancelled;
        booking.CancellationFee = fee;
        booking.CancelledAt = clock.Now;
        bookings.Update(booking);

        invoices.VoidBookingInvoices(booking.Id);
        if (fee > 0)
        {
            invoices.IssueCancellationFee(booking, fee);
        }

        notifications.BookingCancelled(booking, fee);
        return booking;
    }

    public Booking Reschedule(
        string bookingId,
        string actorId,
        bool actorIsAdmin,
        DateTime newStart,
        int newHours)
    {
        var booking = GetChangeable(bookingId, actorId, actorIsAdmin);
        var room = GetRoom(booking.RoomId);
        availability.EnsureBookable(room, newStart, newHours, booking.Attendees, booking.Id);

        var previousStart = booking.Start;
        var quote = pricing.Quote(room, newStart, newHours);
        booking.Start = newStart;
        booking.Hours = newHours;
        booking.Price = quote.Total;
        bookings.Update(booking);

        invoices.VoidBookingInvoices(booking.Id);
        invoices.IssueForBooking(booking, room, quote);
        notifications.BookingRescheduled(booking, room, previousStart);
        return booking;
    }

    public IReadOnlyList<Booking> GetVisibleTo(string userId, bool isAdmin)
    {
        var visible = isAdmin ? bookings.GetAll() : bookings.GetByCustomer(userId);
        return visible.OrderBy(booking => booking.Id, StringComparer.Ordinal).ToList();
    }

    public Booking Get(string bookingId, string userId, bool isAdmin)
    {
        var booking = bookings.GetById(bookingId);
        if (booking is null || (!isAdmin && booking.CustomerId != userId))
        {
            throw new BusinessRuleException(ErrorCodes.NotFound, "Booking not found.");
        }

        return booking;
    }

    public bool CanChange(Booking booking) =>
        booking.Status == BookingStatus.Confirmed && booking.Start > clock.Now;

    private Booking GetChangeable(string bookingId, string actorId, bool actorIsAdmin)
    {
        var booking = Get(bookingId, actorId, actorIsAdmin);
        if (!CanChange(booking))
        {
            throw new BusinessRuleException(
                ErrorCodes.InvalidStatus,
                "Only upcoming confirmed bookings can be changed.");
        }

        return booking;
    }

    private Room GetRoom(string roomId) =>
        rooms.GetById(roomId)
        ?? throw new BusinessRuleException(ErrorCodes.NotFound, "Room not found.");
}
