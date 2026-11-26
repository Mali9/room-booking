using RoomBooking.Core.Services;

namespace RoomBooking.Core.Facilities.Events;

/// <summary>Texts shown to customers of Events.</summary>
public static class EventsMessages
{
    public static string OrderPlaced(string orderId, decimal total) =>
        $"Order {orderId} placed. Total: {Text.Money(total)}.";
}
