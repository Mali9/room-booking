using RoomBooking.Core.Facilities.Pantry;

namespace RoomBooking.Infrastructure.Facilities.Pantry;

/// <summary>Seed data of Pantry: three items and two orders of Customer Sara.</summary>
public static class PantrySeed
{
    public static List<PantryItem> Items() =>
    [
        new() { Id = "PAN-A", Name = "Pantry Standard", UnitPrice = 215m, Capacity = 50 },
        new() { Id = "PAN-B", Name = "Pantry Premium", UnitPrice = 225m, Capacity = 30 },
        new() { Id = "PAN-C", Name = "Pantry Limited", UnitPrice = 110m, Capacity = 3 },
    ];

    public static List<PantryOrder> Orders() =>
    [
        new() { Id = "PAN-1001", CustomerId = "U-2", ItemId = "PAN-A", Quantity = 2, Total = 523.25m },
        new() { Id = "PAN-1002", CustomerId = "U-2", ItemId = "PAN-B", Quantity = 1, Total = 287.50m },
    ];
}
