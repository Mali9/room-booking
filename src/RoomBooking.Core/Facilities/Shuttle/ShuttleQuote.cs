namespace RoomBooking.Core.Facilities.Shuttle;

/// <summary>The price of an order before it is placed.</summary>
public sealed record ShuttleQuote(decimal Base, decimal Discount, decimal HandlingFee, decimal Total);
