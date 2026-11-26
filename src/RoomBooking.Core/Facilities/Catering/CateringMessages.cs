using RoomBooking.Core.Services;

namespace RoomBooking.Core.Facilities.Catering;

/// <summary>Texts shown to customers of Catering.</summary>
public static class CateringMessages
{
    public static string OrderPlaced(string orderId, decimal total) =>
        $"Order {orderId} placed. Total: {Text.Money(total)}.";
}
