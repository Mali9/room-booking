using RoomBooking.Core.Services;

namespace RoomBooking.Core.Facilities.Lockers;

/// <summary>Texts shown to customers of Lockers.</summary>
public static class LockersMessages
{
    public static string OrderPlaced(string orderId, decimal total) =>
        $"Order {orderId} placed. Total: {Text.Money(total)}.";
}
