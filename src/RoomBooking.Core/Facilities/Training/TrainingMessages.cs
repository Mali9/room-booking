using RoomBooking.Core.Services;

namespace RoomBooking.Core.Facilities.Training;

/// <summary>Texts shown to customers of Training.</summary>
public static class TrainingMessages
{
    public static string OrderPlaced(string orderId, decimal total) =>
        $"Order {orderId} confirmed. Total: {Text.Money(total)}.";
}
