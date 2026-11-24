using RoomBooking.Core.Domain;
using RoomBooking.Core.Services;

namespace RoomBooking.Infrastructure;

/// <summary>
/// Process-local data store. Data is deterministic: every start and every Reset()
/// returns to the seed in <see cref="SeedData"/>.
/// </summary>
public sealed class InMemoryDataStore
{
    private readonly PasswordHasher _hasher;

    public InMemoryDataStore(PasswordHasher hasher)
    {
        _hasher = hasher;
        Reset();
    }

    public object Sync { get; } = new();

    internal List<Room> Rooms { get; } = [];
    internal List<Booking> Bookings { get; } = [];
    internal List<Invoice> Invoices { get; } = [];
    internal List<User> Users { get; } = [];
    internal List<Notification> Notifications { get; } = [];

    internal int NextRoomNumber { get; set; }
    internal int NextBookingNumber { get; set; }
    internal int NextInvoiceNumber { get; set; }
    internal int NextNotificationId { get; set; }

    public void Reset()
    {
        lock (Sync)
        {
            Rooms.Clear();
            Bookings.Clear();
            Invoices.Clear();
            Users.Clear();
            Notifications.Clear();
            SeedData.Apply(this, _hasher);
        }
    }
}
