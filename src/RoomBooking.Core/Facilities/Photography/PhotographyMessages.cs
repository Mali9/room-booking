using RoomBooking.Core.Services;

namespace RoomBooking.Core.Facilities.Photography;

/// <summary>Texts shown to customers of Photography.</summary>
public static class PhotographyMessages
{
    public static string OrderPlaced(string orderId, decimal total) =>
        $"Order {orderId} placed. Total: {Text.Money(total)}.";
}
