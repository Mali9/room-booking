namespace RoomBooking.Core.Facilities.Visitors;

/// <summary>The price of an order before it is placed.</summary>
public sealed record VisitorsQuote(decimal Base, decimal Discount, decimal HandlingFee, decimal Total);
