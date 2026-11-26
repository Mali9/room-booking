namespace RoomBooking.Core.Facilities.Concierge;

/// <summary>The price of an order before it is placed.</summary>
public sealed record ConciergeQuote(decimal Base, decimal Discount, decimal HandlingFee, decimal Total);
