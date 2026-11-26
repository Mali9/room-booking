using RoomBooking.Core.Facilities.Desks;

namespace RoomBooking.Infrastructure.Facilities.Desks;

/// <summary>Seed data of Desks: three items and two orders of Customer Sara.</summary>
public static class DesksSeed
{
    public static List<DesksItem> Items() =>
    [
        new() { Id = "DES-A", Name = "Desks Standard", UnitPrice = 80m, Capacity = 50 },
        new() { Id = "DES-B", Name = "Desks Premium", UnitPrice = 144m, Capacity = 30 },
        new() { Id = "DES-C", Name = "Desks Limited", UnitPrice = 83m, Capacity = 3 },
    ];

    public static List<DesksOrder> Orders() =>
    [
        new() { Id = "DES-1001", CustomerId = "U-2", ItemId = "DES-A", Quantity = 2, Total = 230.00m },
        new() { Id = "DES-1002", CustomerId = "U-2", ItemId = "DES-B", Quantity = 1, Total = 207.00m },
    ];
}
