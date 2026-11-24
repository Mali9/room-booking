using System.Globalization;
using RoomBooking.Core.Abstractions;
using RoomBooking.Core.Services;
using RoomBooking.Infrastructure;

namespace RoomBooking.Web;

public static class ServiceRegistration
{
    public static IServiceCollection AddRoomBooking(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddSingleton<IClock>(CreateClock(configuration["Clock:FixedNow"]));

        services.AddSingleton<PasswordHasher>();
        services.AddSingleton<InMemoryDataStore>();
        services.AddSingleton<IRoomRepository, InMemoryRoomRepository>();
        services.AddSingleton<IBookingRepository, InMemoryBookingRepository>();
        services.AddSingleton<IInvoiceRepository, InMemoryInvoiceRepository>();
        services.AddSingleton<IUserRepository, InMemoryUserRepository>();
        services.AddSingleton<INotificationRepository, InMemoryNotificationRepository>();

        services.AddSingleton<PricingPolicy>();
        services.AddSingleton<CancellationPolicy>();
        services.AddSingleton<PricingService>();
        services.AddSingleton<AvailabilityService>();
        services.AddSingleton<InvoiceService>();
        services.AddSingleton<NotificationService>();
        services.AddSingleton<RoomService>();
        services.AddSingleton<BookingService>();
        services.AddSingleton<AuthService>();
        return services;
    }

    private static IClock CreateClock(string? fixedNow)
    {
        if (string.IsNullOrWhiteSpace(fixedNow))
        {
            return new SystemClock();
        }

        var now = DateTime.Parse(fixedNow, CultureInfo.InvariantCulture, DateTimeStyles.None);
        return new FixedClock(now);
    }
}
