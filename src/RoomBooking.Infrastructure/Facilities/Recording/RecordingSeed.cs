using RoomBooking.Core.Facilities.Recording;

namespace RoomBooking.Infrastructure.Facilities.Recording;

/// <summary>Seed data of Recording: three items and two orders of Customer Sara.</summary>
public static class RecordingSeed
{
    public static List<RecordingItem> Items() =>
    [
        new() { Id = "REC-A", Name = "Recording Standard", UnitPrice = 200m, Capacity = 50 },
        new() { Id = "REC-B", Name = "Recording Premium", UnitPrice = 216m, Capacity = 30 },
        new() { Id = "REC-C", Name = "Recording Limited", UnitPrice = 107m, Capacity = 3 },
    ];

    public static List<RecordingOrder> Orders() =>
    [
        new() { Id = "REC-1001", CustomerId = "U-2", ItemId = "REC-A", Quantity = 2, Total = 552.00m },
        new() { Id = "REC-1002", CustomerId = "U-2", ItemId = "REC-B", Quantity = 1, Total = 298.08m },
    ];
}
