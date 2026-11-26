using RoomBooking.Core.Services;

namespace RoomBooking.Core.Facilities.Keys;

/// <summary>Texts shown to customers of Keys.</summary>
public static class KeysMessages
{
    public static string OrderPlaced(string orderId, decimal total) =>
        $"Order {orderId} placed. Total: {Text.Money(total)}.";
}
