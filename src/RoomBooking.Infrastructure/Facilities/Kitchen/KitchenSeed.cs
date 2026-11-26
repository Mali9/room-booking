using RoomBooking.Core.Facilities.Kitchen;

namespace RoomBooking.Infrastructure.Facilities.Kitchen;

/// <summary>Seed data of Kitchen: three items and two orders of Customer Sara.</summary>
public static class KitchenSeed
{
    public static List<KitchenItem> Items() =>
    [
        new() { Id = "KIT-A", Name = "Kitchen Standard", UnitPrice = 150m, Capacity = 50 },
        new() { Id = "KIT-B", Name = "Kitchen Premium", UnitPrice = 186m, Capacity = 30 },
        new() { Id = "KIT-C", Name = "Kitchen Limited", UnitPrice = 97m, Capacity = 3 },
    ];

    public static List<KitchenOrder> Orders() =>
    [
        new() { Id = "KIT-1001", CustomerId = "U-2", ItemId = "KIT-A", Quantity = 2, Total = 414.00m },
        new() { Id = "KIT-1002", CustomerId = "U-2", ItemId = "KIT-B", Quantity = 1, Total = 256.68m },
    ];
}
