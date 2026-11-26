using RoomBooking.Core.Facilities.Translation;

namespace RoomBooking.Infrastructure.Facilities.Translation;

/// <summary>Seed data of Translation: three items and two orders of Customer Sara.</summary>
public static class TranslationSeed
{
    public static List<TranslationItem> Items() =>
    [
        new() { Id = "TRA-A", Name = "Translation Standard", UnitPrice = 175m, Capacity = 50 },
        new() { Id = "TRA-B", Name = "Translation Premium", UnitPrice = 201m, Capacity = 30 },
        new() { Id = "TRA-C", Name = "Translation Limited", UnitPrice = 102m, Capacity = 3 },
    ];

    public static List<TranslationOrder> Orders() =>
    [
        new() { Id = "TRA-1001", CustomerId = "U-2", ItemId = "TRA-A", Quantity = 2, Total = 508.88m },
        new() { Id = "TRA-1002", CustomerId = "U-2", ItemId = "TRA-B", Quantity = 1, Total = 303.26m },
    ];
}
