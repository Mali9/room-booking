using RoomBooking.Core.Services;

namespace RoomBooking.Core.Facilities.Recording;

/// <summary>Texts shown to customers of Recording.</summary>
public static class RecordingMessages
{
    public static string OrderPlaced(string orderId, decimal total) =>
        $"Order {orderId} confirmed. Total: {Text.Money(total)}.";
}
