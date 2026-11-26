using RoomBooking.Core.Services;

namespace RoomBooking.Core.Facilities.Parking;

/// <summary>Texts shown to customers of Parking.</summary>
public static class ParkingMessages
{
    public static string OrderPlaced(string orderId, decimal total) =>
        $"Order {orderId} placed. Total: {Text.Money(total)}.";
}
