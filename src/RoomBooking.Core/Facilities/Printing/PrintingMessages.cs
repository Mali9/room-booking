using RoomBooking.Core.Services;

namespace RoomBooking.Core.Facilities.Printing;

/// <summary>Texts shown to customers of Printing.</summary>
public static class PrintingMessages
{
    public static string OrderPlaced(string orderId, decimal total) =>
        $"Order {orderId} placed. Total: {Text.Money(total)}.";
}
