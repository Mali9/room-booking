using RoomBooking.Core.Domain;

namespace RoomBooking.Core.Abstractions;

public interface IClock
{
    DateTime Now { get; }
}

public interface IRoomRepository
{
    IReadOnlyList<Room> GetAll();
    Room? GetById(string id);
    string NextId();
    void Add(Room room);
    void Update(Room room);
}

public interface IBookingRepository
{
    IReadOnlyList<Booking> GetAll();
    Booking? GetById(string id);
    IReadOnlyList<Booking> GetByCustomer(string customerId);
    IReadOnlyList<Booking> GetConfirmedForRoom(string roomId);
    string NextId();
    void Add(Booking booking);
    void Update(Booking booking);
}

public interface IInvoiceRepository
{
    IReadOnlyList<Invoice> GetAll();
    Invoice? GetById(string id);
    IReadOnlyList<Invoice> GetByCustomer(string customerId);
    IReadOnlyList<Invoice> GetByBooking(string bookingId);
    string NextId();
    void Add(Invoice invoice);
    void Update(Invoice invoice);
}

public interface IUserRepository
{
    User? GetById(string id);
    User? GetByEmail(string email);
}

public interface INotificationRepository
{
    IReadOnlyList<Notification> GetForUser(string userId);
    void Add(string userId, NotificationType type, string message, DateTime createdAt);
    void MarkAllRead(string userId);
}
