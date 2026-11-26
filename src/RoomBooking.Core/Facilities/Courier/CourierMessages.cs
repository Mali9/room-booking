using RoomBooking.Core.Services;

namespace RoomBooking.Core.Facilities.Courier;

/// <summary>Texts shown to customers of Courier.</summary>
public static class CourierMessages
{
    public static string OrderPlaced(string orderId, decimal total) =>
        $"Order {orderId} confirmed. Total: {Text.Money(total)}.";
}
