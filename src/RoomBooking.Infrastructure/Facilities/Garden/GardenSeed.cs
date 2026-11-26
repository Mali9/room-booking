using RoomBooking.Core.Facilities.Garden;

namespace RoomBooking.Infrastructure.Facilities.Garden;

/// <summary>Seed data of Garden: three items and two orders of Customer Sara.</summary>
public static class GardenSeed
{
    public static List<GardenItem> Items() =>
    [
        new() { Id = "GAR-A", Name = "Garden Standard", UnitPrice = 155m, Capacity = 50 },
        new() { Id = "GAR-B", Name = "Garden Premium", UnitPrice = 189m, Capacity = 30 },
        new() { Id = "GAR-C", Name = "Garden Limited", UnitPrice = 98m, Capacity = 3 },
    ];

    public static List<GardenOrder> Orders() =>
    [
        new() { Id = "GAR-1001", CustomerId = "U-2", ItemId = "GAR-A", Quantity = 2, Total = 460.00m },
        new() { Id = "GAR-1002", CustomerId = "U-2", ItemId = "GAR-B", Quantity = 1, Total = 286.06m },
    ];
}
