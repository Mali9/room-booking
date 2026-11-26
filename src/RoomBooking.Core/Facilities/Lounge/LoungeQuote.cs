namespace RoomBooking.Core.Facilities.Lounge;

/// <summary>The price of an order before it is placed.</summary>
public sealed record LoungeQuote(decimal Base, decimal Discount, decimal HandlingFee, decimal Total);
