using RoomBooking.Core.Facilities.Childcare;

namespace RoomBooking.Infrastructure.Facilities.Childcare;

/// <summary>Seed data of Childcare: three items and two orders of Customer Sara.</summary>
public static class ChildcareSeed
{
    public static List<ChildcareItem> Items() =>
    [
        new() { Id = "CHI-A", Name = "Childcare Standard", UnitPrice = 110m, Capacity = 50 },
        new() { Id = "CHI-B", Name = "Childcare Premium", UnitPrice = 162m, Capacity = 30 },
        new() { Id = "CHI-C", Name = "Childcare Limited", UnitPrice = 89m, Capacity = 3 },
    ];

    public static List<ChildcareOrder> Orders() =>
    [
        new() { Id = "CHI-1001", CustomerId = "U-2", ItemId = "CHI-A", Quantity = 2, Total = 379.50m },
        new() { Id = "CHI-1002", CustomerId = "U-2", ItemId = "CHI-B", Quantity = 1, Total = 279.45m },
    ];
}
