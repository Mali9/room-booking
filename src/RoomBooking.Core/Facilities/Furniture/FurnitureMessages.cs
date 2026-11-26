using RoomBooking.Core.Services;

namespace RoomBooking.Core.Facilities.Furniture;

/// <summary>Texts shown to customers of Furniture.</summary>
public static class FurnitureMessages
{
    public static string OrderPlaced(string orderId, decimal total) =>
        $"Order {orderId} confirmed. Total: {Text.Money(total)}.";
}
