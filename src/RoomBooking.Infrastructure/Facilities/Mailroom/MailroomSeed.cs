using RoomBooking.Core.Facilities.Mailroom;

namespace RoomBooking.Infrastructure.Facilities.Mailroom;

/// <summary>Seed data of Mailroom: three items and two orders of Customer Sara.</summary>
public static class MailroomSeed
{
    public static List<MailroomItem> Items() =>
    [
        new() { Id = "MA6-A", Name = "Mailroom Standard", UnitPrice = 120m, Capacity = 50 },
        new() { Id = "MA6-B", Name = "Mailroom Premium", UnitPrice = 168m, Capacity = 30 },
        new() { Id = "MA6-C", Name = "Mailroom Limited", UnitPrice = 91m, Capacity = 3 },
    ];

    public static List<MailroomOrder> Orders() =>
    [
        new() { Id = "MA6-1001", CustomerId = "U-2", ItemId = "MA6-A", Quantity = 2, Total = 303.60m },
        new() { Id = "MA6-1002", CustomerId = "U-2", ItemId = "MA6-B", Quantity = 1, Total = 212.52m },
    ];
}
