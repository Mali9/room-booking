namespace RoomBooking.Core.Facilities.Pantry;

/// <summary>The price of an order before it is placed.</summary>
public sealed record PantryQuote(decimal Base, decimal Discount, decimal HandlingFee, decimal Total);
