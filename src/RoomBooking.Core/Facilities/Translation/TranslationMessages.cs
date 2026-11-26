using RoomBooking.Core.Services;

namespace RoomBooking.Core.Facilities.Translation;

/// <summary>Texts shown to customers of Translation.</summary>
public static class TranslationMessages
{
    public static string OrderPlaced(string orderId, decimal total) =>
        $"Order {orderId} placed. Total: {Text.Money(total)}.";
}
