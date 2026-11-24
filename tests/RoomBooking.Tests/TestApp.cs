using RoomBooking.Core.Abstractions;
using RoomBooking.Core.Services;
using RoomBooking.Infrastructure;

namespace RoomBooking.Tests;

/// <summary>The real services over a freshly seeded store, with the clock frozen at the seed "now".</summary>
public sealed class TestApp
{
    public TestApp()
        : this(SeedData.DefaultNow)
    {
    }

    public TestApp(DateTime now)
    {
        Clock = new FixedClock(now);
        Hasher = new PasswordHasher();
        Store = new InMemoryDataStore(Hasher);
        Rooms = new InMemoryRoomRepository(Store);
        Bookings = new InMemoryBookingRepository(Store);
        Invoices = new InMemoryInvoiceRepository(Store);
        Users = new InMemoryUserRepository(Store);
        NotificationStore = new InMemoryNotificationRepository(Store);

        Pricing = new PricingService(new PricingPolicy());
        Cancellation = new CancellationPolicy();
        Availability = new AvailabilityService(Rooms, Bookings, Clock);
        InvoiceService = new InvoiceService(Invoices, Pricing, Clock);
        Notifications = new NotificationService(NotificationStore, Clock);
        RoomService = new RoomService(Rooms, Bookings, Clock);
        BookingService = new BookingService(
            Bookings, Rooms, Availability, Pricing, Cancellation, InvoiceService, Notifications, Clock);
        Auth = new AuthService(Users, Hasher);
    }

    public IClock Clock { get; }
    public PasswordHasher Hasher { get; }
    public InMemoryDataStore Store { get; }
    public IRoomRepository Rooms { get; }
    public IBookingRepository Bookings { get; }
    public IInvoiceRepository Invoices { get; }
    public IUserRepository Users { get; }
    public INotificationRepository NotificationStore { get; }
    public PricingService Pricing { get; }
    public CancellationPolicy Cancellation { get; }
    public AvailabilityService Availability { get; }
    public InvoiceService InvoiceService { get; }
    public NotificationService Notifications { get; }
    public RoomService RoomService { get; }
    public BookingService BookingService { get; }
    public AuthService Auth { get; }

    public static DateTime At(int day, int hour) => new(2027, 1, day, hour, 0, 0);
}
