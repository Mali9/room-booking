namespace RoomBooking.Core.Facilities.Bikes;

/// <summary>The price of an order before it is placed.</summary>
public sealed record BikesQuote(decimal Base, decimal Discount, decimal HandlingFee, decimal Total);
