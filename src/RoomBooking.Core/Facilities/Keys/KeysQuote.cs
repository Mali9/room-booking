namespace RoomBooking.Core.Facilities.Keys;

/// <summary>The price of an order before it is placed.</summary>
public sealed record KeysQuote(decimal Base, decimal Discount, decimal HandlingFee, decimal Total);
