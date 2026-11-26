using RoomBooking.Core.Facilities.ItSupport;

namespace RoomBooking.Infrastructure.Facilities.ItSupport;

/// <summary>Seed data of ItSupport: three items and two orders of Customer Sara.</summary>
public static class ItSupportSeed
{
    public static List<ItSupportItem> Items() =>
    [
        new() { Id = "ITS-A", Name = "ItSupport Standard", UnitPrice = 180m, Capacity = 50 },
        new() { Id = "ITS-B", Name = "ItSupport Premium", UnitPrice = 204m, Capacity = 30 },
        new() { Id = "ITS-C", Name = "ItSupport Limited", UnitPrice = 103m, Capacity = 3 },
    ];

    public static List<ItSupportOrder> Orders() =>
    [
        new() { Id = "ITS-1001", CustomerId = "U-2", ItemId = "ITS-A", Quantity = 2, Total = 517.50m },
        new() { Id = "ITS-1002", CustomerId = "U-2", ItemId = "ITS-B", Quantity = 1, Total = 293.25m },
    ];
}
