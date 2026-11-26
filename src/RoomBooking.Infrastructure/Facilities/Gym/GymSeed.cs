using RoomBooking.Core.Facilities.Gym;

namespace RoomBooking.Infrastructure.Facilities.Gym;

/// <summary>Seed data of Gym: three items and two orders of Customer Sara.</summary>
public static class GymSeed
{
    public static List<GymItem> Items() =>
    [
        new() { Id = "GYM-A", Name = "Gym Standard", UnitPrice = 105m, Capacity = 50 },
        new() { Id = "GYM-B", Name = "Gym Premium", UnitPrice = 159m, Capacity = 30 },
        new() { Id = "GYM-C", Name = "Gym Limited", UnitPrice = 88m, Capacity = 3 },
    ];

    public static List<GymOrder> Orders() =>
    [
        new() { Id = "GYM-1001", CustomerId = "U-2", ItemId = "GYM-A", Quantity = 2, Total = 301.88m },
        new() { Id = "GYM-1002", CustomerId = "U-2", ItemId = "GYM-B", Quantity = 1, Total = 228.56m },
    ];
}
