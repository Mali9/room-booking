using RoomBooking.Core.Facilities.Lounge;

namespace RoomBooking.Infrastructure.Facilities.Lounge;

/// <summary>Seed data of Lounge: three items and two orders of Customer Sara.</summary>
public static class LoungeSeed
{
    public static List<LoungeItem> Items() =>
    [
        new() { Id = "LOU-A", Name = "Lounge Standard", UnitPrice = 100m, Capacity = 50 },
        new() { Id = "LOU-B", Name = "Lounge Premium", UnitPrice = 156m, Capacity = 30 },
        new() { Id = "LOU-C", Name = "Lounge Limited", UnitPrice = 87m, Capacity = 3 },
    ];

    public static List<LoungeOrder> Orders() =>
    [
        new() { Id = "LOU-1001", CustomerId = "U-2", ItemId = "LOU-A", Quantity = 2, Total = 276.00m },
        new() { Id = "LOU-1002", CustomerId = "U-2", ItemId = "LOU-B", Quantity = 1, Total = 215.28m },
    ];
}
