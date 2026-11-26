using RoomBooking.Core.Facilities.Storage;

namespace RoomBooking.Infrastructure.Facilities.Storage;

/// <summary>Seed data of Storage: three items and two orders of Customer Sara.</summary>
public static class StorageSeed
{
    public static List<StorageItem> Items() =>
    [
        new() { Id = "STO-A", Name = "Storage Standard", UnitPrice = 125m, Capacity = 50 },
        new() { Id = "STO-B", Name = "Storage Premium", UnitPrice = 171m, Capacity = 30 },
        new() { Id = "STO-C", Name = "Storage Limited", UnitPrice = 92m, Capacity = 3 },
    ];

    public static List<StorageOrder> Orders() =>
    [
        new() { Id = "STO-1001", CustomerId = "U-2", ItemId = "STO-A", Quantity = 2, Total = 345.00m },
        new() { Id = "STO-1002", CustomerId = "U-2", ItemId = "STO-B", Quantity = 1, Total = 235.98m },
    ];
}
