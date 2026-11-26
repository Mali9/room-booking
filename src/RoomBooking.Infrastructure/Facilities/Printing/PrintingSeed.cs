using RoomBooking.Core.Facilities.Printing;

namespace RoomBooking.Infrastructure.Facilities.Printing;

/// <summary>Seed data of Printing: three items and two orders of Customer Sara.</summary>
public static class PrintingSeed
{
    public static List<PrintingItem> Items() =>
    [
        new() { Id = "PRI-A", Name = "Printing Standard", UnitPrice = 90m, Capacity = 50 },
        new() { Id = "PRI-B", Name = "Printing Premium", UnitPrice = 150m, Capacity = 30 },
        new() { Id = "PRI-C", Name = "Printing Limited", UnitPrice = 85m, Capacity = 3 },
    ];

    public static List<PrintingOrder> Orders() =>
    [
        new() { Id = "PRI-1001", CustomerId = "U-2", ItemId = "PRI-A", Quantity = 2, Total = 207.00m },
        new() { Id = "PRI-1002", CustomerId = "U-2", ItemId = "PRI-B", Quantity = 1, Total = 172.50m },
    ];
}
