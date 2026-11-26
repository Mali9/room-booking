using RoomBooking.Core.Facilities.Cleaning;

namespace RoomBooking.Infrastructure.Facilities.Cleaning;

/// <summary>Seed data of Cleaning: three items and two orders of Customer Sara.</summary>
public static class CleaningSeed
{
    public static List<CleaningItem> Items() =>
    [
        new() { Id = "CLE-A", Name = "Cleaning Standard", UnitPrice = 60m, Capacity = 50 },
        new() { Id = "CLE-B", Name = "Cleaning Premium", UnitPrice = 132m, Capacity = 30 },
        new() { Id = "CLE-C", Name = "Cleaning Limited", UnitPrice = 79m, Capacity = 3 },
    ];

    public static List<CleaningOrder> Orders() =>
    [
        new() { Id = "CLE-1001", CustomerId = "U-2", ItemId = "CLE-A", Quantity = 2, Total = 207.00m },
        new() { Id = "CLE-1002", CustomerId = "U-2", ItemId = "CLE-B", Quantity = 1, Total = 227.70m },
    ];
}
