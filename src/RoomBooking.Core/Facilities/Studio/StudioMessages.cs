using RoomBooking.Core.Services;

namespace RoomBooking.Core.Facilities.Studio;

/// <summary>Texts shown to customers of Studio.</summary>
public static class StudioMessages
{
    public static string OrderPlaced(string orderId, decimal total) =>
        $"Order {orderId} confirmed. Total: {Text.Money(total)}.";
}
