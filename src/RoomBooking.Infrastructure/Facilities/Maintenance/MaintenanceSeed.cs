using RoomBooking.Core.Facilities.Maintenance;

namespace RoomBooking.Infrastructure.Facilities.Maintenance;

/// <summary>Seed data of Maintenance: three items and two orders of Customer Sara.</summary>
public static class MaintenanceSeed
{
    public static List<MaintenanceItem> Items() =>
    [
        new() { Id = "MAI-A", Name = "Maintenance Standard", UnitPrice = 65m, Capacity = 50 },
        new() { Id = "MAI-B", Name = "Maintenance Premium", UnitPrice = 135m, Capacity = 30 },
        new() { Id = "MAI-C", Name = "Maintenance Limited", UnitPrice = 80m, Capacity = 3 },
    ];

    public static List<MaintenanceOrder> Orders() =>
    [
        new() { Id = "MAI-1001", CustomerId = "U-2", ItemId = "MAI-A", Quantity = 2, Total = 149.50m },
        new() { Id = "MAI-1002", CustomerId = "U-2", ItemId = "MAI-B", Quantity = 1, Total = 155.25m },
    ];
}
