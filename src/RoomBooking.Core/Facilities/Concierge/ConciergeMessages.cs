using RoomBooking.Core.Services;

namespace RoomBooking.Core.Facilities.Concierge;

/// <summary>Texts shown to customers of Concierge.</summary>
public static class ConciergeMessages
{
    public static string OrderPlaced(string orderId, decimal total) =>
        $"Order {orderId} confirmed. Total: {Text.Money(total)}.";
}
