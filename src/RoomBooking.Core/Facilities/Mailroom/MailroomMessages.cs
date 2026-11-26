using RoomBooking.Core.Services;

namespace RoomBooking.Core.Facilities.Mailroom;

/// <summary>Texts shown to customers of Mailroom.</summary>
public static class MailroomMessages
{
    public static string OrderPlaced(string orderId, decimal total) =>
        $"Order {orderId} confirmed. Total: {Text.Money(total)}.";
}
