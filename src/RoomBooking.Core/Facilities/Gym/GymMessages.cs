using RoomBooking.Core.Services;

namespace RoomBooking.Core.Facilities.Gym;

/// <summary>Texts shown to customers of Gym.</summary>
public static class GymMessages
{
    public static string OrderPlaced(string orderId, decimal total) =>
        $"Order {orderId} confirmed. Total: {Text.Money(total)}.";
}
