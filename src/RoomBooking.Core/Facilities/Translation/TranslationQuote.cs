namespace RoomBooking.Core.Facilities.Translation;

/// <summary>The price of an order before it is placed.</summary>
public sealed record TranslationQuote(decimal Base, decimal Discount, decimal HandlingFee, decimal Total);
