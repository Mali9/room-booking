using RoomBooking.Core.Facilities.Catering;

namespace RoomBooking.Infrastructure.Facilities.Catering;

/// <summary>Seed data of Catering: three items and two orders of Customer Sara.</summary>
public static class CateringSeed
{
    public static List<CateringItem> Items() =>
    [
        new() { Id = "CAT-A", Name = "Catering Standard", UnitPrice = 40m, Capacity = 50 },
        new() { Id = "CAT-B", Name = "Catering Premium", UnitPrice = 120m, Capacity = 30 },
        new() { Id = "CAT-C", Name = "Catering Limited", UnitPrice = 75m, Capacity = 3 },
    ];

    public static List<CateringOrder> Orders() =>
    [
        new() { Id = "CAT-1001", CustomerId = "U-2", ItemId = "CAT-A", Quantity = 2, Total = 92.00m },
        new() { Id = "CAT-1002", CustomerId = "U-2", ItemId = "CAT-B", Quantity = 1, Total = 138.00m },
    ];
}
