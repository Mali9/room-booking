using RoomBooking.Core.Facilities.Concierge;

namespace RoomBooking.Infrastructure.Facilities.Concierge;

/// <summary>Seed data of Concierge: three items and two orders of Customer Sara.</summary>
public static class ConciergeSeed
{
    public static List<ConciergeItem> Items() =>
    [
        new() { Id = "CON-A", Name = "Concierge Standard", UnitPrice = 210m, Capacity = 50 },
        new() { Id = "CON-B", Name = "Concierge Premium", UnitPrice = 222m, Capacity = 30 },
        new() { Id = "CON-C", Name = "Concierge Limited", UnitPrice = 109m, Capacity = 3 },
    ];

    public static List<ConciergeOrder> Orders() =>
    [
        new() { Id = "CON-1001", CustomerId = "U-2", ItemId = "CON-A", Quantity = 2, Total = 724.50m },
        new() { Id = "CON-1002", CustomerId = "U-2", ItemId = "CON-B", Quantity = 1, Total = 382.95m },
    ];
}
