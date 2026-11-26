using RoomBooking.Core.Services;

namespace RoomBooking.Core.Facilities.Archive;

/// <summary>Texts shown to customers of Archive.</summary>
public static class ArchiveMessages
{
    public static string OrderPlaced(string orderId, decimal total) =>
        $"Order {orderId} placed. Total: {Text.Money(total)}.";
}
