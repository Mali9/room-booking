using RoomBooking.Core.Facilities.Keys;

namespace RoomBooking.Infrastructure.Facilities.Keys;

/// <summary>Seed data of Keys: three items and two orders of Customer Sara.</summary>
public static class KeysSeed
{
    public static List<KeysItem> Items() =>
    [
        new() { Id = "KEY-A", Name = "Keys Standard", UnitPrice = 225m, Capacity = 50 },
        new() { Id = "KEY-B", Name = "Keys Premium", UnitPrice = 231m, Capacity = 30 },
        new() { Id = "KEY-C", Name = "Keys Limited", UnitPrice = 112m, Capacity = 3 },
    ];

    public static List<KeysOrder> Orders() =>
    [
        new() { Id = "KEY-1001", CustomerId = "U-2", ItemId = "KEY-A", Quantity = 2, Total = 621.00m },
        new() { Id = "KEY-1002", CustomerId = "U-2", ItemId = "KEY-B", Quantity = 1, Total = 318.78m },
    ];
}
