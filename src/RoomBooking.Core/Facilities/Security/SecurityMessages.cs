using RoomBooking.Core.Services;

namespace RoomBooking.Core.Facilities.Security;

/// <summary>Texts shown to customers of Security.</summary>
public static class SecurityMessages
{
    public static string OrderPlaced(string orderId, decimal total) =>
        $"Order {orderId} placed. Total: {Text.Money(total)}.";
}
