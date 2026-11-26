using RoomBooking.Core.Facilities.Bikes;

namespace RoomBooking.Infrastructure.Facilities.Bikes;

/// <summary>Seed data of Bikes: three items and two orders of Customer Sara.</summary>
public static class BikesSeed
{
    public static List<BikesItem> Items() =>
    [
        new() { Id = "BIK-A", Name = "Bikes Standard", UnitPrice = 165m, Capacity = 50 },
        new() { Id = "BIK-B", Name = "Bikes Premium", UnitPrice = 195m, Capacity = 30 },
        new() { Id = "BIK-C", Name = "Bikes Limited", UnitPrice = 100m, Capacity = 3 },
    ];

    public static List<BikesOrder> Orders() =>
    [
        new() { Id = "BIK-1001", CustomerId = "U-2", ItemId = "BIK-A", Quantity = 2, Total = 379.50m },
        new() { Id = "BIK-1002", CustomerId = "U-2", ItemId = "BIK-B", Quantity = 1, Total = 224.25m },
    ];
}
