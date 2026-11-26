using RoomBooking.Core.Services;

namespace RoomBooking.Core.Facilities.Uniforms;

/// <summary>Texts shown to customers of Uniforms.</summary>
public static class UniformsMessages
{
    public static string OrderPlaced(string orderId, decimal total) =>
        $"Order {orderId} placed. Total: {Text.Money(total)}.";
}
