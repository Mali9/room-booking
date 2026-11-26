using RoomBooking.Core.Facilities.Laundry;

namespace RoomBooking.Infrastructure.Facilities.Laundry;

/// <summary>Seed data of Laundry: three items and two orders of Customer Sara.</summary>
public static class LaundrySeed
{
    public static List<LaundryItem> Items() =>
    [
        new() { Id = "LAU-A", Name = "Laundry Standard", UnitPrice = 115m, Capacity = 50 },
        new() { Id = "LAU-B", Name = "Laundry Premium", UnitPrice = 165m, Capacity = 30 },
        new() { Id = "LAU-C", Name = "Laundry Limited", UnitPrice = 90m, Capacity = 3 },
    ];

    public static List<LaundryOrder> Orders() =>
    [
        new() { Id = "LAU-1001", CustomerId = "U-2", ItemId = "LAU-A", Quantity = 2, Total = 276.00m },
        new() { Id = "LAU-1002", CustomerId = "U-2", ItemId = "LAU-B", Quantity = 1, Total = 201.25m },
    ];
}
