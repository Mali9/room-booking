namespace RoomBooking.Core.Facilities.Events;

/// <summary>The price of an order before it is placed.</summary>
public sealed record EventsQuote(decimal Base, decimal Discount, decimal HandlingFee, decimal Total);
