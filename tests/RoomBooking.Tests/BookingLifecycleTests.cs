using RoomBooking.Core;
using RoomBooking.Core.Domain;
using RoomBooking.Core.Services;
using RoomBooking.Infrastructure;
using static RoomBooking.Tests.TestApp;

namespace RoomBooking.Tests;

public sealed class BookingLifecycleTests
{
    private readonly TestApp _app = new();

    private Booking Book(string roomId, DateTime start, int hours, int attendees) =>
        _app.BookingService.Create(new BookingRequest(SeedData.SaraId, roomId, start, hours, attendees));

    [Fact]
    [Trait("TestCase", "TC-016")]
    [Trait("Requirement", "REQ-07")]
    public void Customer_books_room_at_peak_price()
    {
        var booking = Book("R-102", At(7, 10), 2, 6);

        Assert.Equal("BK-1005", booking.Id);
        Assert.Equal(BookingStatus.Confirmed, booking.Status);
        Assert.Equal(600m, booking.Price);
    }

    [Fact]
    [Trait("TestCase", "TC-021")]
    [Trait("Requirement", "REQ-10")]
    public void Booking_issues_invoice_with_discount_and_vat()
    {
        var booking = Book("R-101", At(7, 9), 4, 4);

        var invoice = Assert.Single(_app.Invoices.GetByBooking(booking.Id));
        Assert.Equal("INV-5005", invoice.Id);
        Assert.Equal(
            [
                new InvoiceLine("Al Nakheel, 4 h (4 peak, 0 off-peak)", 600m),
                new InvoiceLine("Long booking discount (10%)", -60m),
            ],
            invoice.Lines);
        Assert.Equal(540m, invoice.Subtotal);
        Assert.Equal(81m, invoice.Vat);
        Assert.Equal(621m, invoice.Total);
    }

    [Fact]
    [Trait("TestCase", "TC-022")]
    [Trait("Requirement", "REQ-10")]
    public void Seeded_invoice_shows_discount_and_vat()
    {
        var invoice = _app.InvoiceService.Get("INV-5003", SeedData.OmarId, isAdmin: false);

        Assert.Equal([2400m, -240m], invoice.Lines.Select(line => line.Amount));
        Assert.Equal(2160m, invoice.Subtotal);
        Assert.Equal(324m, invoice.Vat);
        Assert.Equal(2484m, invoice.Total);
    }

    [Fact]
    [Trait("TestCase", "TC-023")]
    [Trait("Requirement", "REQ-11")]
    public void Cancelling_more_than_24_hours_ahead_is_free()
    {
        Assert.Equal(0m, _app.BookingService.PreviewCancellationFee("BK-1002", SeedData.SaraId, false));

        var booking = _app.BookingService.Cancel("BK-1002", SeedData.SaraId, actorIsAdmin: false);

        Assert.Equal(BookingStatus.Cancelled, booking.Status);
        Assert.Equal(0m, booking.CancellationFee);
        var invoice = Assert.Single(_app.Invoices.GetByBooking("BK-1002"));
        Assert.Equal(InvoiceStatus.Void, invoice.Status);
    }

    [Fact]
    [Trait("TestCase", "TC-024")]
    [Trait("Requirement", "REQ-11")]
    public void Late_cancellation_charges_half_the_price()
    {
        var booking = _app.BookingService.Cancel("BK-1001", SeedData.SaraId, actorIsAdmin: false);

        Assert.Equal(150m, booking.CancellationFee);
        var invoices = _app.Invoices.GetByBooking("BK-1001");
        Assert.Equal(InvoiceStatus.Void, invoices.Single(invoice => invoice.Id == "INV-5001").Status);
        var fee = invoices.Single(invoice => invoice.Kind == InvoiceKind.CancellationFee);
        Assert.Equal("INV-5005", fee.Id);
        Assert.Equal(172.5m, fee.Total);
    }

    [Fact]
    [Trait("TestCase", "TC-025")]
    [Trait("Requirement", "REQ-15")]
    public void Admin_cancellation_waives_the_fee()
    {
        var booking = _app.BookingService.Cancel("BK-1001", SeedData.AdminId, actorIsAdmin: true);

        Assert.Equal(BookingStatus.Cancelled, booking.Status);
        Assert.Equal(0m, booking.CancellationFee);
    }

    [Fact]
    [Trait("TestCase", "TC-026")]
    [Trait("Requirement", "REQ-12")]
    public void Reschedule_reprices_and_reissues_invoice()
    {
        var booking = _app.BookingService.Reschedule("BK-1002", SeedData.SaraId, false, At(8, 10), 2);

        Assert.Equal(At(8, 10), booking.Start);
        Assert.Equal(400m, booking.Price);
        var invoices = _app.Invoices.GetByBooking("BK-1002");
        Assert.Equal(InvoiceStatus.Void, invoices.Single(invoice => invoice.Id == "INV-5002").Status);
        Assert.Equal(460m, invoices.Single(invoice => invoice.Id == "INV-5005").Total);
    }

    [Fact]
    [Trait("TestCase", "TC-027")]
    [Trait("Requirement", "REQ-12")]
    public void Reschedule_into_a_booked_slot_is_rejected()
    {
        var error = Assert.Throws<BusinessRuleException>(
            () => _app.BookingService.Reschedule("BK-1002", SeedData.SaraId, false, At(7, 15), 1));

        Assert.Equal("Room Al Waha is already booked at that time.", error.Message);
        Assert.Equal(At(6, 13), _app.Bookings.GetById("BK-1002")!.Start);
    }

    [Fact]
    [Trait("TestCase", "TC-028")]
    [Trait("Requirement", "REQ-13")]
    public void Booking_sends_confirmation_notification()
    {
        Assert.Equal(0, _app.Notifications.UnreadCount(SeedData.SaraId));

        Book("R-102", At(7, 10), 2, 6);

        Assert.Equal(1, _app.Notifications.UnreadCount(SeedData.SaraId));
        Assert.Equal(
            "Booking BK-1005 confirmed: Al Waha on 2027-01-07 from 10:00 to 12:00.",
            _app.Notifications.GetForUser(SeedData.SaraId)[0].Message);
    }

    [Fact]
    [Trait("TestCase", "TC-029")]
    [Trait("Requirement", "REQ-13")]
    public void Cancellation_sends_notification_with_fee()
    {
        _app.BookingService.Cancel("BK-1002", SeedData.SaraId, actorIsAdmin: false);

        Assert.Equal(
            "Booking BK-1002 cancelled. Cancellation fee: 0.00 SAR.",
            _app.Notifications.GetForUser(SeedData.SaraId)[0].Message);
    }

    [Fact]
    [Trait("TestCase", "TC-030")]
    [Trait("Requirement", "REQ-14")]
    public void Customer_sees_only_own_bookings()
    {
        var ids = _app.BookingService.GetVisibleTo(SeedData.SaraId, isAdmin: false).Select(b => b.Id);

        Assert.Equal(["BK-1001", "BK-1002"], ids);
    }

    [Fact]
    [Trait("TestCase", "TC-031")]
    [Trait("Requirement", "REQ-14")]
    public void Admin_sees_all_bookings()
    {
        var ids = _app.BookingService.GetVisibleTo(SeedData.AdminId, isAdmin: true).Select(b => b.Id);

        Assert.Equal(["BK-1001", "BK-1002", "BK-1003", "BK-1004"], ids);
    }

    [Fact]
    [Trait("TestCase", "TC-032")]
    [Trait("Requirement", "REQ-14")]
    public void Customer_cannot_open_another_customers_booking()
    {
        var error = Assert.Throws<BusinessRuleException>(
            () => _app.BookingService.Get("BK-1003", SeedData.SaraId, isAdmin: false));

        Assert.Equal("Booking not found.", error.Message);
    }
}
