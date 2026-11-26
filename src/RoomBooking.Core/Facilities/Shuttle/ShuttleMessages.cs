using RoomBooking.Core.Services;

namespace RoomBooking.Core.Facilities.Shuttle;

/// <summary>Texts shown to customers of Shuttle.</summary>
public static class ShuttleMessages
{
    public static string OrderPlaced(string orderId, decimal total) =>
        $"Order {orderId} placed. Total: {Text.Money(total)}.";
}
