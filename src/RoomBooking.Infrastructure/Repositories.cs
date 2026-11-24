using RoomBooking.Core.Abstractions;
using RoomBooking.Core.Domain;

namespace RoomBooking.Infrastructure;

public sealed class InMemoryRoomRepository(InMemoryDataStore store) : IRoomRepository
{
    public IReadOnlyList<Room> GetAll()
    {
        lock (store.Sync)
        {
            return store.Rooms.ToList();
        }
    }

    public Room? GetById(string id)
    {
        lock (store.Sync)
        {
            return store.Rooms.FirstOrDefault(room => room.Id == id);
        }
    }

    public string NextId()
    {
        lock (store.Sync)
        {
            return $"R-{store.NextRoomNumber++}";
        }
    }

    public void Add(Room room)
    {
        lock (store.Sync)
        {
            store.Rooms.Add(room);
        }
    }

    public void Update(Room room)
    {
        // Entities are held by reference; nothing else to persist.
    }
}

public sealed class InMemoryBookingRepository(InMemoryDataStore store) : IBookingRepository
{
    public IReadOnlyList<Booking> GetAll()
    {
        lock (store.Sync)
        {
            return store.Bookings.ToList();
        }
    }

    public Booking? GetById(string id)
    {
        lock (store.Sync)
        {
            return store.Bookings.FirstOrDefault(booking => booking.Id == id);
        }
    }

    public IReadOnlyList<Booking> GetByCustomer(string customerId)
    {
        lock (store.Sync)
        {
            return store.Bookings.Where(booking => booking.CustomerId == customerId).ToList();
        }
    }

    public IReadOnlyList<Booking> GetConfirmedForRoom(string roomId)
    {
        lock (store.Sync)
        {
            return store.Bookings
                .Where(booking => booking.RoomId == roomId && booking.Status == BookingStatus.Confirmed)
                .ToList();
        }
    }

    public string NextId()
    {
        lock (store.Sync)
        {
            return $"BK-{store.NextBookingNumber++}";
        }
    }

    public void Add(Booking booking)
    {
        lock (store.Sync)
        {
            store.Bookings.Add(booking);
        }
    }

    public void Update(Booking booking)
    {
        // Entities are held by reference; nothing else to persist.
    }
}

public sealed class InMemoryInvoiceRepository(InMemoryDataStore store) : IInvoiceRepository
{
    public IReadOnlyList<Invoice> GetAll()
    {
        lock (store.Sync)
        {
            return store.Invoices.ToList();
        }
    }

    public Invoice? GetById(string id)
    {
        lock (store.Sync)
        {
            return store.Invoices.FirstOrDefault(invoice => invoice.Id == id);
        }
    }

    public IReadOnlyList<Invoice> GetByCustomer(string customerId)
    {
        lock (store.Sync)
        {
            return store.Invoices.Where(invoice => invoice.CustomerId == customerId).ToList();
        }
    }

    public IReadOnlyList<Invoice> GetByBooking(string bookingId)
    {
        lock (store.Sync)
        {
            return store.Invoices.Where(invoice => invoice.BookingId == bookingId).ToList();
        }
    }

    public string NextId()
    {
        lock (store.Sync)
        {
            return $"INV-{store.NextInvoiceNumber++}";
        }
    }

    public void Add(Invoice invoice)
    {
        lock (store.Sync)
        {
            store.Invoices.Add(invoice);
        }
    }

    public void Update(Invoice invoice)
    {
        // Entities are held by reference; nothing else to persist.
    }
}

public sealed class InMemoryUserRepository(InMemoryDataStore store) : IUserRepository
{
    public User? GetById(string id)
    {
        lock (store.Sync)
        {
            return store.Users.FirstOrDefault(user => user.Id == id);
        }
    }

    public User? GetByEmail(string email)
    {
        lock (store.Sync)
        {
            return store.Users.FirstOrDefault(
                user => string.Equals(user.Email, email, StringComparison.OrdinalIgnoreCase));
        }
    }
}

public sealed class InMemoryNotificationRepository(InMemoryDataStore store) : INotificationRepository
{
    public IReadOnlyList<Notification> GetForUser(string userId)
    {
        lock (store.Sync)
        {
            return store.Notifications.Where(notification => notification.UserId == userId).ToList();
        }
    }

    public void Add(string userId, NotificationType type, string message, DateTime createdAt)
    {
        lock (store.Sync)
        {
            store.Notifications.Add(new Notification
            {
                Id = store.NextNotificationId++,
                UserId = userId,
                Type = type,
                Message = message,
                CreatedAt = createdAt,
            });
        }
    }

    public void MarkAllRead(string userId)
    {
        lock (store.Sync)
        {
            foreach (var notification in store.Notifications.Where(item => item.UserId == userId))
            {
                notification.IsRead = true;
            }
        }
    }
}
