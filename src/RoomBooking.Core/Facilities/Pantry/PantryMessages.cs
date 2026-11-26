using RoomBooking.Core.Services;

namespace RoomBooking.Core.Facilities.Pantry;

/// <summary>Texts shown to customers of Pantry.</summary>
public static class PantryMessages
{
    public static string OrderPlaced(string orderId, decimal total) =>
        $"Order {orderId} placed. Total: {Text.Money(total)}.";
}
