using RoomBooking.Core.Facilities.Archive;

namespace RoomBooking.Infrastructure.Facilities.Archive;

/// <summary>Seed data of Archive: three items and two orders of Customer Sara.</summary>
public static class ArchiveSeed
{
    public static List<ArchiveItem> Items() =>
    [
        new() { Id = "ARC-A", Name = "Archive Standard", UnitPrice = 235m, Capacity = 50 },
        new() { Id = "ARC-B", Name = "Archive Premium", UnitPrice = 237m, Capacity = 30 },
        new() { Id = "ARC-C", Name = "Archive Limited", UnitPrice = 114m, Capacity = 3 },
    ];

    public static List<ArchiveOrder> Orders() =>
    [
        new() { Id = "ARC-1001", CustomerId = "U-2", ItemId = "ARC-A", Quantity = 2, Total = 830.88m },
        new() { Id = "ARC-1002", CustomerId = "U-2", ItemId = "ARC-B", Quantity = 1, Total = 428.95m },
    ];
}
