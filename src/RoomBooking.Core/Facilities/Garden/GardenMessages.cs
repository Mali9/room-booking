using RoomBooking.Core.Services;

namespace RoomBooking.Core.Facilities.Garden;

/// <summary>Texts shown to customers of Garden.</summary>
public static class GardenMessages
{
    public static string OrderPlaced(string orderId, decimal total) =>
        $"Order {orderId} confirmed. Total: {Text.Money(total)}.";
}
