using RoomBooking.Core.Facilities.Wellness;

namespace RoomBooking.Infrastructure.Facilities.Wellness;

/// <summary>Seed data of Wellness: three items and two orders of Customer Sara.</summary>
public static class WellnessSeed
{
    public static List<WellnessItem> Items() =>
    [
        new() { Id = "WEL-A", Name = "Wellness Standard", UnitPrice = 130m, Capacity = 50 },
        new() { Id = "WEL-B", Name = "Wellness Premium", UnitPrice = 174m, Capacity = 30 },
        new() { Id = "WEL-C", Name = "Wellness Limited", UnitPrice = 93m, Capacity = 3 },
    ];

    public static List<WellnessOrder> Orders() =>
    [
        new() { Id = "WEL-1001", CustomerId = "U-2", ItemId = "WEL-A", Quantity = 2, Total = 373.75m },
        new() { Id = "WEL-1002", CustomerId = "U-2", ItemId = "WEL-B", Quantity = 1, Total = 250.13m },
    ];
}
