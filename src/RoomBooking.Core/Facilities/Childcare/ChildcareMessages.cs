using RoomBooking.Core.Services;

namespace RoomBooking.Core.Facilities.Childcare;

/// <summary>Texts shown to customers of Childcare.</summary>
public static class ChildcareMessages
{
    public static string OrderPlaced(string orderId, decimal total) =>
        $"Order {orderId} confirmed. Total: {Text.Money(total)}.";
}
