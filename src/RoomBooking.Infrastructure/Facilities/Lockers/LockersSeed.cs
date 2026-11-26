using RoomBooking.Core.Facilities.Lockers;

namespace RoomBooking.Infrastructure.Facilities.Lockers;

/// <summary>Seed data of Lockers: three items and two orders of Customer Sara.</summary>
public static class LockersSeed
{
    public static List<LockersItem> Items() =>
    [
        new() { Id = "LOC-A", Name = "Lockers Standard", UnitPrice = 75m, Capacity = 50 },
        new() { Id = "LOC-B", Name = "Lockers Premium", UnitPrice = 141m, Capacity = 30 },
        new() { Id = "LOC-C", Name = "Lockers Limited", UnitPrice = 82m, Capacity = 3 },
    ];

    public static List<LockersOrder> Orders() =>
    [
        new() { Id = "LOC-1001", CustomerId = "U-2", ItemId = "LOC-A", Quantity = 2, Total = 235.75m },
        new() { Id = "LOC-1002", CustomerId = "U-2", ItemId = "LOC-B", Quantity = 1, Total = 223.33m },
    ];
}
