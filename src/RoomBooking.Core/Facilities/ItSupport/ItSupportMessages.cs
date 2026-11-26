using RoomBooking.Core.Services;

namespace RoomBooking.Core.Facilities.ItSupport;

/// <summary>Texts shown to customers of ItSupport.</summary>
public static class ItSupportMessages
{
    public static string OrderPlaced(string orderId, decimal total) =>
        $"Order {orderId} placed. Total: {Text.Money(total)}.";
}
