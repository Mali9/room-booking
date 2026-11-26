using RoomBooking.Core.Services;

namespace RoomBooking.Core.Facilities.Badges;

/// <summary>Texts shown to customers of Badges.</summary>
public static class BadgesMessages
{
    public static string OrderPlaced(string orderId, decimal total) =>
        $"Order {orderId} placed. Total: {Text.Money(total)}.";
}
