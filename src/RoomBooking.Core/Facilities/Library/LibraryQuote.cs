namespace RoomBooking.Core.Facilities.Library;

/// <summary>The price of an order before it is placed.</summary>
public sealed record LibraryQuote(decimal Base, decimal Discount, decimal HandlingFee, decimal Total);
