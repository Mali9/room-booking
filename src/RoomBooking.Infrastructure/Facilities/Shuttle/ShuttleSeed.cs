using RoomBooking.Core.Facilities.Shuttle;

namespace RoomBooking.Infrastructure.Facilities.Shuttle;

/// <summary>Seed data of Shuttle: three items and two orders of Customer Sara.</summary>
public static class ShuttleSeed
{
    public static List<ShuttleItem> Items() =>
    [
        new() { Id = "SHU-A", Name = "Shuttle Standard", UnitPrice = 95m, Capacity = 50 },
        new() { Id = "SHU-B", Name = "Shuttle Premium", UnitPrice = 153m, Capacity = 30 },
        new() { Id = "SHU-C", Name = "Shuttle Limited", UnitPrice = 86m, Capacity = 3 },
    ];

    public static List<ShuttleOrder> Orders() =>
    [
        new() { Id = "SHU-1001", CustomerId = "U-2", ItemId = "SHU-A", Quantity = 2, Total = 260.48m },
        new() { Id = "SHU-1002", CustomerId = "U-2", ItemId = "SHU-B", Quantity = 1, Total = 213.67m },
    ];
}
