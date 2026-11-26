using RoomBooking.Core.Services;

namespace RoomBooking.Core.Facilities.Visitors;

/// <summary>Texts shown to customers of Visitors.</summary>
public static class VisitorsMessages
{
    public static string OrderPlaced(string orderId, decimal total) =>
        $"Order {orderId} placed. Total: {Text.Money(total)}.";
}
