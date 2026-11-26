using RoomBooking.Core.Facilities.Courier;

namespace RoomBooking.Infrastructure.Facilities.Courier;

/// <summary>Seed data of Courier: three items and two orders of Customer Sara.</summary>
public static class CourierSeed
{
    public static List<CourierItem> Items() =>
    [
        new() { Id = "COU-A", Name = "Courier Standard", UnitPrice = 170m, Capacity = 50 },
        new() { Id = "COU-B", Name = "Courier Premium", UnitPrice = 198m, Capacity = 30 },
        new() { Id = "COU-C", Name = "Courier Limited", UnitPrice = 101m, Capacity = 3 },
    ];

    public static List<CourierOrder> Orders() =>
    [
        new() { Id = "COU-1001", CustomerId = "U-2", ItemId = "COU-A", Quantity = 2, Total = 430.10m },
        new() { Id = "COU-1002", CustomerId = "U-2", ItemId = "COU-B", Quantity = 1, Total = 250.47m },
    ];
}
