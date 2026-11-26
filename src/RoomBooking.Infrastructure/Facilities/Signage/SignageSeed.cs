using RoomBooking.Core.Facilities.Signage;

namespace RoomBooking.Infrastructure.Facilities.Signage;

/// <summary>Seed data of Signage: three items and two orders of Customer Sara.</summary>
public static class SignageSeed
{
    public static List<SignageItem> Items() =>
    [
        new() { Id = "SIG-A", Name = "Signage Standard", UnitPrice = 190m, Capacity = 50 },
        new() { Id = "SIG-B", Name = "Signage Premium", UnitPrice = 210m, Capacity = 30 },
        new() { Id = "SIG-C", Name = "Signage Limited", UnitPrice = 105m, Capacity = 3 },
    ];

    public static List<SignageOrder> Orders() =>
    [
        new() { Id = "SIG-1001", CustomerId = "U-2", ItemId = "SIG-A", Quantity = 2, Total = 437.00m },
        new() { Id = "SIG-1002", CustomerId = "U-2", ItemId = "SIG-B", Quantity = 1, Total = 241.50m },
    ];
}
