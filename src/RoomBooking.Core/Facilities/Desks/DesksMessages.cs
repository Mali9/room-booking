using RoomBooking.Core.Services;

namespace RoomBooking.Core.Facilities.Desks;

/// <summary>Texts shown to customers of Desks.</summary>
public static class DesksMessages
{
    public static string OrderPlaced(string orderId, decimal total) =>
        $"Order {orderId} placed. Total: {Text.Money(total)}.";
}
