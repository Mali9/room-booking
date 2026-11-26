using RoomBooking.Core.Facilities.Studio;

namespace RoomBooking.Infrastructure.Facilities.Studio;

/// <summary>Seed data of Studio: three items and two orders of Customer Sara.</summary>
public static class StudioSeed
{
    public static List<StudioItem> Items() =>
    [
        new() { Id = "STU-A", Name = "Studio Standard", UnitPrice = 145m, Capacity = 50 },
        new() { Id = "STU-B", Name = "Studio Premium", UnitPrice = 183m, Capacity = 30 },
        new() { Id = "STU-C", Name = "Studio Limited", UnitPrice = 96m, Capacity = 3 },
    ];

    public static List<StudioOrder> Orders() =>
    [
        new() { Id = "STU-1001", CustomerId = "U-2", ItemId = "STU-A", Quantity = 2, Total = 366.85m },
        new() { Id = "STU-1002", CustomerId = "U-2", ItemId = "STU-B", Quantity = 1, Total = 231.50m },
    ];
}
