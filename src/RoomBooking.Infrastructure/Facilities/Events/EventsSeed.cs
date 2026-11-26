using RoomBooking.Core.Facilities.Events;

namespace RoomBooking.Infrastructure.Facilities.Events;

/// <summary>Seed data of Events: three items and two orders of Customer Sara.</summary>
public static class EventsSeed
{
    public static List<EventsItem> Items() =>
    [
        new() { Id = "EVE-A", Name = "Events Standard", UnitPrice = 85m, Capacity = 50 },
        new() { Id = "EVE-B", Name = "Events Premium", UnitPrice = 147m, Capacity = 30 },
        new() { Id = "EVE-C", Name = "Events Limited", UnitPrice = 84m, Capacity = 3 },
    ];

    public static List<EventsOrder> Orders() =>
    [
        new() { Id = "EVE-1001", CustomerId = "U-2", ItemId = "EVE-A", Quantity = 2, Total = 293.25m },
        new() { Id = "EVE-1002", CustomerId = "U-2", ItemId = "EVE-B", Quantity = 1, Total = 253.58m },
    ];
}
