using RoomBooking.Core.Services;

namespace RoomBooking.Core.Facilities.Kitchen;

/// <summary>Texts shown to customers of Kitchen.</summary>
public static class KitchenMessages
{
    public static string OrderPlaced(string orderId, decimal total) =>
        $"Order {orderId} confirmed. Total: {Text.Money(total)}.";
}
