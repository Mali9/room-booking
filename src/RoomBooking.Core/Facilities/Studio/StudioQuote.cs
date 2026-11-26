namespace RoomBooking.Core.Facilities.Studio;

/// <summary>The price of an order before it is placed.</summary>
public sealed record StudioQuote(decimal Base, decimal Discount, decimal HandlingFee, decimal Total);
