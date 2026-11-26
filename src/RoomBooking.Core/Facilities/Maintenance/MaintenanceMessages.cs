using RoomBooking.Core.Services;

namespace RoomBooking.Core.Facilities.Maintenance;

/// <summary>Texts shown to customers of Maintenance.</summary>
public static class MaintenanceMessages
{
    public static string OrderPlaced(string orderId, decimal total) =>
        $"Order {orderId} placed. Total: {Text.Money(total)}.";
}
