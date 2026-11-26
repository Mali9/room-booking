using RoomBooking.Core.Services;

namespace RoomBooking.Core.Facilities.Signage;

/// <summary>Texts shown to customers of Signage.</summary>
public static class SignageMessages
{
    public static string OrderPlaced(string orderId, decimal total) =>
        $"Order {orderId} confirmed. Total: {Text.Money(total)}.";
}
