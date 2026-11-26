using RoomBooking.Core.Facilities.Visitors;

namespace RoomBooking.Infrastructure.Facilities.Visitors;

/// <summary>Seed data of Visitors: three items and two orders of Customer Sara.</summary>
public static class VisitorsSeed
{
    public static List<VisitorsItem> Items() =>
    [
        new() { Id = "VIS-A", Name = "Visitors Standard", UnitPrice = 55m, Capacity = 50 },
        new() { Id = "VIS-B", Name = "Visitors Premium", UnitPrice = 129m, Capacity = 30 },
        new() { Id = "VIS-C", Name = "Visitors Limited", UnitPrice = 78m, Capacity = 3 },
    ];

    public static List<VisitorsOrder> Orders() =>
    [
        new() { Id = "VIS-1001", CustomerId = "U-2", ItemId = "VIS-A", Quantity = 2, Total = 175.38m },
        new() { Id = "VIS-1002", CustomerId = "U-2", ItemId = "VIS-B", Quantity = 1, Total = 202.69m },
    ];
}
