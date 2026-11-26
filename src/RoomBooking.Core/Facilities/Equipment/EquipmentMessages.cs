using RoomBooking.Core.Services;

namespace RoomBooking.Core.Facilities.Equipment;

/// <summary>Texts shown to customers of Equipment.</summary>
public static class EquipmentMessages
{
    public static string OrderPlaced(string orderId, decimal total) =>
        $"Order {orderId} placed. Total: {Text.Money(total)}.";
}
