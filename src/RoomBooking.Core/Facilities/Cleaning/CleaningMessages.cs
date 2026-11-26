using RoomBooking.Core.Services;

namespace RoomBooking.Core.Facilities.Cleaning;

/// <summary>Texts shown to customers of Cleaning.</summary>
public static class CleaningMessages
{
    public static string OrderPlaced(string orderId, decimal total) =>
        $"Order {orderId} placed. Total: {Text.Money(total)}.";
}
