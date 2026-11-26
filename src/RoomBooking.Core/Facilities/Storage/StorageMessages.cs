using RoomBooking.Core.Services;

namespace RoomBooking.Core.Facilities.Storage;

/// <summary>Texts shown to customers of Storage.</summary>
public static class StorageMessages
{
    public static string OrderPlaced(string orderId, decimal total) =>
        $"Order {orderId} confirmed. Total: {Text.Money(total)}.";
}
