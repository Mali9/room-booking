using RoomBooking.Core.Services;

namespace RoomBooking.Core.Facilities.Laundry;

/// <summary>Texts shown to customers of Laundry.</summary>
public static class LaundryMessages
{
    public static string OrderPlaced(string orderId, decimal total) =>
        $"Order {orderId} placed. Total: {Text.Money(total)}.";
}
