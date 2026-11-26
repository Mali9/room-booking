using RoomBooking.Core.Services;

namespace RoomBooking.Core.Facilities.Bikes;

/// <summary>Texts shown to customers of Bikes.</summary>
public static class BikesMessages
{
    public static string OrderPlaced(string orderId, decimal total) =>
        $"Order {orderId} placed. Total: {Text.Money(total)}.";
}
