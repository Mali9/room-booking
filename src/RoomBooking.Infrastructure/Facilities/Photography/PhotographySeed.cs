using RoomBooking.Core.Facilities.Photography;

namespace RoomBooking.Infrastructure.Facilities.Photography;

/// <summary>Seed data of Photography: three items and two orders of Customer Sara.</summary>
public static class PhotographySeed
{
    public static List<PhotographyItem> Items() =>
    [
        new() { Id = "PHO-A", Name = "Photography Standard", UnitPrice = 195m, Capacity = 50 },
        new() { Id = "PHO-B", Name = "Photography Premium", UnitPrice = 213m, Capacity = 30 },
        new() { Id = "PHO-C", Name = "Photography Limited", UnitPrice = 106m, Capacity = 3 },
    ];

    public static List<PhotographyOrder> Orders() =>
    [
        new() { Id = "PHO-1001", CustomerId = "U-2", ItemId = "PHO-A", Quantity = 2, Total = 510.60m },
        new() { Id = "PHO-1002", CustomerId = "U-2", ItemId = "PHO-B", Quantity = 1, Total = 286.70m },
    ];
}
