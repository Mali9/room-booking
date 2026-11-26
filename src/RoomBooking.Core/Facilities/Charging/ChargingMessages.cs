using RoomBooking.Core.Services;

namespace RoomBooking.Core.Facilities.Charging;

/// <summary>Texts shown to customers of Charging.</summary>
public static class ChargingMessages
{
    public static string OrderPlaced(string orderId, decimal total) =>
        $"Order {orderId} confirmed. Total: {Text.Money(total)}.";
}
