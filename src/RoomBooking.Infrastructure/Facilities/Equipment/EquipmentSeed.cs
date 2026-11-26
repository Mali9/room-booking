using RoomBooking.Core.Facilities.Equipment;

namespace RoomBooking.Infrastructure.Facilities.Equipment;

/// <summary>Seed data of Equipment: three items and two orders of Customer Sara.</summary>
public static class EquipmentSeed
{
    public static List<EquipmentItem> Items() =>
    [
        new() { Id = "EQU-A", Name = "Equipment Standard", UnitPrice = 50m, Capacity = 50 },
        new() { Id = "EQU-B", Name = "Equipment Premium", UnitPrice = 126m, Capacity = 30 },
        new() { Id = "EQU-C", Name = "Equipment Limited", UnitPrice = 77m, Capacity = 3 },
    ];

    public static List<EquipmentOrder> Orders() =>
    [
        new() { Id = "EQU-1001", CustomerId = "U-2", ItemId = "EQU-A", Quantity = 2, Total = 138.00m },
        new() { Id = "EQU-1002", CustomerId = "U-2", ItemId = "EQU-B", Quantity = 1, Total = 173.88m },
    ];
}
