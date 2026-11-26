using RoomBooking.Core.Services;

namespace RoomBooking.Core.Facilities.Wellness;

/// <summary>Texts shown to customers of Wellness.</summary>
public static class WellnessMessages
{
    public static string OrderPlaced(string orderId, decimal total) =>
        $"Order {orderId} confirmed. Total: {Text.Money(total)}.";
}
