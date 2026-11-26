using RoomBooking.Core.Facilities.Furniture;

namespace RoomBooking.Infrastructure.Facilities.Furniture;

/// <summary>Seed data of Furniture: three items and two orders of Customer Sara.</summary>
public static class FurnitureSeed
{
    public static List<FurnitureItem> Items() =>
    [
        new() { Id = "FUR-A", Name = "Furniture Standard", UnitPrice = 185m, Capacity = 50 },
        new() { Id = "FUR-B", Name = "Furniture Premium", UnitPrice = 207m, Capacity = 30 },
        new() { Id = "FUR-C", Name = "Furniture Limited", UnitPrice = 104m, Capacity = 3 },
    ];

    public static List<FurnitureOrder> Orders() =>
    [
        new() { Id = "FUR-1001", CustomerId = "U-2", ItemId = "FUR-A", Quantity = 2, Total = 638.25m },
        new() { Id = "FUR-1002", CustomerId = "U-2", ItemId = "FUR-B", Quantity = 1, Total = 357.08m },
    ];
}
