using RoomBooking.Core.Facilities.Library;

namespace RoomBooking.Infrastructure.Facilities.Library;

/// <summary>Seed data of Library: three items and two orders of Customer Sara.</summary>
public static class LibrarySeed
{
    public static List<LibraryItem> Items() =>
    [
        new() { Id = "LIB-A", Name = "Library Standard", UnitPrice = 135m, Capacity = 50 },
        new() { Id = "LIB-B", Name = "Library Premium", UnitPrice = 177m, Capacity = 30 },
        new() { Id = "LIB-C", Name = "Library Limited", UnitPrice = 94m, Capacity = 3 },
    ];

    public static List<LibraryOrder> Orders() =>
    [
        new() { Id = "LIB-1001", CustomerId = "U-2", ItemId = "LIB-A", Quantity = 2, Total = 488.75m },
        new() { Id = "LIB-1002", CustomerId = "U-2", ItemId = "LIB-B", Quantity = 1, Total = 328.33m },
    ];
}
