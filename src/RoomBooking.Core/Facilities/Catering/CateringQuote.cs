namespace RoomBooking.Core.Facilities.Catering;

/// <summary>The price of an order before it is placed.</summary>
public sealed record CateringQuote(decimal Base, decimal Discount, decimal HandlingFee, decimal Total);
