using RoomBooking.Core.Services;

namespace RoomBooking.Core.Facilities.Labs;

/// <summary>Texts shown to customers of Labs.</summary>
public static class LabsMessages
{
    public static string OrderPlaced(string orderId, decimal total) =>
        $"Order {orderId} confirmed. Total: {Text.Money(total)}.";
}
