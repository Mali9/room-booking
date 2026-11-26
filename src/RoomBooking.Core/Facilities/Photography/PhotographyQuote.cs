namespace RoomBooking.Core.Facilities.Photography;

/// <summary>The price of an order before it is placed.</summary>
public sealed record PhotographyQuote(decimal Base, decimal Discount, decimal HandlingFee, decimal Total);
