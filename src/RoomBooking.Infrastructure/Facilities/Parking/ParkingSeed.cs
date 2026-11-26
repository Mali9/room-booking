using RoomBooking.Core.Facilities.Parking;

namespace RoomBooking.Infrastructure.Facilities.Parking;

/// <summary>Seed data of Parking: three items and two orders of Customer Sara.</summary>
public static class ParkingSeed
{
    public static List<ParkingItem> Items() =>
    [
        new() { Id = "PAR-A", Name = "Parking Standard", UnitPrice = 45m, Capacity = 50 },
        new() { Id = "PAR-B", Name = "Parking Premium", UnitPrice = 123m, Capacity = 30 },
        new() { Id = "PAR-C", Name = "Parking Limited", UnitPrice = 76m, Capacity = 3 },
    ];

    public static List<ParkingOrder> Orders() =>
    [
        new() { Id = "PAR-1001", CustomerId = "U-2", ItemId = "PAR-A", Quantity = 2, Total = 113.85m },
        new() { Id = "PAR-1002", CustomerId = "U-2", ItemId = "PAR-B", Quantity = 1, Total = 155.60m },
    ];
}
