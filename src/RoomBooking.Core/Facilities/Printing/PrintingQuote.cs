namespace RoomBooking.Core.Facilities.Printing;

/// <summary>The price of an order before it is placed.</summary>
public sealed record PrintingQuote(decimal Base, decimal Discount, decimal HandlingFee, decimal Total);
