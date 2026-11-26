using RoomBooking.Core.Facilities.Labs;

namespace RoomBooking.Infrastructure.Facilities.Labs;

/// <summary>Seed data of Labs: three items and two orders of Customer Sara.</summary>
public static class LabsSeed
{
    public static List<LabsItem> Items() =>
    [
        new() { Id = "LAB-A", Name = "Labs Standard", UnitPrice = 140m, Capacity = 50 },
        new() { Id = "LAB-B", Name = "Labs Premium", UnitPrice = 180m, Capacity = 30 },
        new() { Id = "LAB-C", Name = "Labs Limited", UnitPrice = 95m, Capacity = 3 },
    ];

    public static List<LabsOrder> Orders() =>
    [
        new() { Id = "LAB-1001", CustomerId = "U-2", ItemId = "LAB-A", Quantity = 2, Total = 322.00m },
        new() { Id = "LAB-1002", CustomerId = "U-2", ItemId = "LAB-B", Quantity = 1, Total = 207.00m },
    ];
}
