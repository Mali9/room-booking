using RoomBooking.Core.Services;

namespace RoomBooking.Core.Facilities.Library;

/// <summary>Texts shown to customers of Library.</summary>
public static class LibraryMessages
{
    public static string OrderPlaced(string orderId, decimal total) =>
        $"Order {orderId} confirmed. Total: {Text.Money(total)}.";
}
