using RoomBooking.Core.Domain;
using RoomBooking.Core.Services;

namespace RoomBooking.Infrastructure;

/// <summary>
/// Deterministic test data. Amounts are stored as issued (historical invoices do not
/// change when pricing rules change). Documented in docs/test-data.md.
/// </summary>
public static class SeedData
{
    public const string AdminId = "U-1";
    public const string SaraId = "U-2";
    public const string OmarId = "U-3";

    public const string AdminEmail = "admin@roombooking.test";
    public const string AdminPassword = "Admin#2027";
    public const string SaraEmail = "sara@roombooking.test";
    public const string SaraPassword = "Sara#2027";
    public const string OmarEmail = "omar@roombooking.test";
    public const string OmarPassword = "Omar#2027";

    /// <summary>The frozen "now" used by test environments: Monday 2027-01-04 08:00.</summary>
    public static readonly DateTime DefaultNow = new(2027, 1, 4, 8, 0, 0);

    internal static void Apply(InMemoryDataStore store, PasswordHasher hasher)
    {
        store.Users.AddRange(
        [
            new User
            {
                Id = AdminId,
                Email = AdminEmail,
                DisplayName = "Facilities Admin",
                PasswordHash = hasher.Hash(AdminPassword),
                Role = UserRole.Admin,
            },
            new User
            {
                Id = SaraId,
                Email = SaraEmail,
                DisplayName = "Sara Al-Harbi",
                PasswordHash = hasher.Hash(SaraPassword),
                Role = UserRole.Customer,
            },
            new User
            {
                Id = OmarId,
                Email = OmarEmail,
                DisplayName = "Omar Haddad",
                PasswordHash = hasher.Hash(OmarPassword),
                Role = UserRole.Customer,
            },
        ]);

        store.Rooms.AddRange(
        [
            new Room { Id = "R-101", Name = "Al Nakheel", Capacity = 6, HourlyRate = 100m },
            new Room { Id = "R-102", Name = "Al Waha", Capacity = 12, HourlyRate = 200m },
            new Room { Id = "R-201", Name = "Board Room", Capacity = 20, HourlyRate = 400m },
            new Room { Id = "R-301", Name = "Focus Pod", Capacity = 2, HourlyRate = 50m, IsActive = false },
        ]);

        var created = new DateTime(2027, 1, 2, 9, 0, 0);
        AddBooking(store, "BK-1001", "R-101", SaraId, new DateTime(2027, 1, 4, 10, 0, 0), 2, 4, created,
            "INV-5001", [new("Al Nakheel, 2 h (2 peak, 0 off-peak)", 300m)], 300m, 45m,
            "Booking BK-1001 confirmed: Al Nakheel on 2027-01-04 from 10:00 to 12:00.");
        AddBooking(store, "BK-1002", "R-102", SaraId, new DateTime(2027, 1, 6, 13, 0, 0), 2, 8, created,
            "INV-5002", [new("Al Waha, 2 h (2 peak, 0 off-peak)", 600m)], 600m, 90m,
            "Booking BK-1002 confirmed: Al Waha on 2027-01-06 from 13:00 to 15:00.");
        AddBooking(store, "BK-1003", "R-201", OmarId, new DateTime(2027, 1, 5, 9, 0, 0), 4, 15, created,
            "INV-5003",
            [new("Board Room, 4 h (4 peak, 0 off-peak)", 2400m), new("Long booking discount (10%)", -240m)],
            2160m, 324m,
            "Booking BK-1003 confirmed: Board Room on 2027-01-05 from 09:00 to 13:00.");
        AddBooking(store, "BK-1004", "R-102", OmarId, new DateTime(2027, 1, 7, 14, 0, 0), 2, 10, created,
            "INV-5004", [new("Al Waha, 2 h (2 peak, 0 off-peak)", 600m)], 600m, 90m,
            "Booking BK-1004 confirmed: Al Waha on 2027-01-07 from 14:00 to 16:00.");

        store.NextRoomNumber = 401;
        store.NextBookingNumber = 1005;
        store.NextInvoiceNumber = 5005;
        store.NextNotificationId = store.Notifications.Count + 1;
    }

    private static void AddBooking(
        InMemoryDataStore store,
        string bookingId,
        string roomId,
        string customerId,
        DateTime start,
        int hours,
        int attendees,
        DateTime created,
        string invoiceId,
        List<InvoiceLine> lines,
        decimal subtotal,
        decimal vat,
        string notification)
    {
        store.Bookings.Add(new Booking
        {
            Id = bookingId,
            RoomId = roomId,
            CustomerId = customerId,
            Start = start,
            Hours = hours,
            Attendees = attendees,
            Price = subtotal,
            CreatedAt = created,
        });
        store.Invoices.Add(new Invoice
        {
            Id = invoiceId,
            BookingId = bookingId,
            CustomerId = customerId,
            Kind = InvoiceKind.Booking,
            IssuedAt = created,
            Lines = lines,
            Subtotal = subtotal,
            Vat = vat,
        });
        store.Notifications.Add(new Notification
        {
            Id = store.Notifications.Count + 1,
            UserId = customerId,
            Type = NotificationType.BookingConfirmed,
            Message = notification,
            CreatedAt = created,
            IsRead = true,
        });
    }
}
