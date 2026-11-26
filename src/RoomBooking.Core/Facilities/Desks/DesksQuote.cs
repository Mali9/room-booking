namespace RoomBooking.Core.Facilities.Desks;

/// <summary>The price of an order before it is placed.</summary>
public sealed record DesksQuote(decimal Base, decimal Discount, decimal HandlingFee, decimal Total);
