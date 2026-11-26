using RoomBooking.Core.Facilities.Uniforms;

namespace RoomBooking.Infrastructure.Facilities.Uniforms;

/// <summary>Seed data of Uniforms: three items and two orders of Customer Sara.</summary>
public static class UniformsSeed
{
    public static List<UniformsItem> Items() =>
    [
        new() { Id = "UNI-A", Name = "Uniforms Standard", UnitPrice = 220m, Capacity = 50 },
        new() { Id = "UNI-B", Name = "Uniforms Premium", UnitPrice = 228m, Capacity = 30 },
        new() { Id = "UNI-C", Name = "Uniforms Limited", UnitPrice = 111m, Capacity = 3 },
    ];

    public static List<UniformsOrder> Orders() =>
    [
        new() { Id = "UNI-1001", CustomerId = "U-2", ItemId = "UNI-A", Quantity = 2, Total = 556.60m },
        new() { Id = "UNI-1002", CustomerId = "U-2", ItemId = "UNI-B", Quantity = 1, Total = 288.42m },
    ];
}
