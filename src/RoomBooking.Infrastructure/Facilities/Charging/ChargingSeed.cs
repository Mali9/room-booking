using RoomBooking.Core.Facilities.Charging;

namespace RoomBooking.Infrastructure.Facilities.Charging;

/// <summary>Seed data of Charging: three items and two orders of Customer Sara.</summary>
public static class ChargingSeed
{
    public static List<ChargingItem> Items() =>
    [
        new() { Id = "CHA-A", Name = "Charging Standard", UnitPrice = 160m, Capacity = 50 },
        new() { Id = "CHA-B", Name = "Charging Premium", UnitPrice = 192m, Capacity = 30 },
        new() { Id = "CHA-C", Name = "Charging Limited", UnitPrice = 99m, Capacity = 3 },
    ];

    public static List<ChargingOrder> Orders() =>
    [
        new() { Id = "CHA-1001", CustomerId = "U-2", ItemId = "CHA-A", Quantity = 2, Total = 552.00m },
        new() { Id = "CHA-1002", CustomerId = "U-2", ItemId = "CHA-B", Quantity = 1, Total = 331.20m },
    ];
}
