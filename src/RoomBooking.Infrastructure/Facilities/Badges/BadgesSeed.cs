using RoomBooking.Core.Facilities.Badges;

namespace RoomBooking.Infrastructure.Facilities.Badges;

/// <summary>Seed data of Badges: three items and two orders of Customer Sara.</summary>
public static class BadgesSeed
{
    public static List<BadgesItem> Items() =>
    [
        new() { Id = "BAD-A", Name = "Badges Standard", UnitPrice = 230m, Capacity = 50 },
        new() { Id = "BAD-B", Name = "Badges Premium", UnitPrice = 234m, Capacity = 30 },
        new() { Id = "BAD-C", Name = "Badges Limited", UnitPrice = 113m, Capacity = 3 },
    ];

    public static List<BadgesOrder> Orders() =>
    [
        new() { Id = "BAD-1001", CustomerId = "U-2", ItemId = "BAD-A", Quantity = 2, Total = 661.25m },
        new() { Id = "BAD-1002", CustomerId = "U-2", ItemId = "BAD-B", Quantity = 1, Total = 336.38m },
    ];
}
