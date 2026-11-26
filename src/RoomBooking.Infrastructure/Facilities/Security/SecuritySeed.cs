using RoomBooking.Core.Facilities.Security;

namespace RoomBooking.Infrastructure.Facilities.Security;

/// <summary>Seed data of Security: three items and two orders of Customer Sara.</summary>
public static class SecuritySeed
{
    public static List<SecurityItem> Items() =>
    [
        new() { Id = "SEC-A", Name = "Security Standard", UnitPrice = 70m, Capacity = 50 },
        new() { Id = "SEC-B", Name = "Security Premium", UnitPrice = 138m, Capacity = 30 },
        new() { Id = "SEC-C", Name = "Security Limited", UnitPrice = 81m, Capacity = 3 },
    ];

    public static List<SecurityOrder> Orders() =>
    [
        new() { Id = "SEC-1001", CustomerId = "U-2", ItemId = "SEC-A", Quantity = 2, Total = 177.10m },
        new() { Id = "SEC-1002", CustomerId = "U-2", ItemId = "SEC-B", Quantity = 1, Total = 174.57m },
    ];
}
