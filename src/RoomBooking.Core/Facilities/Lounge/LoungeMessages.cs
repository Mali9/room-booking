using RoomBooking.Core.Services;

namespace RoomBooking.Core.Facilities.Lounge;

/// <summary>Texts shown to customers of Lounge.</summary>
public static class LoungeMessages
{
    public static string OrderPlaced(string orderId, decimal total) =>
        $"Order {orderId} placed. Total: {Text.Money(total)}.";
}
