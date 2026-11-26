namespace RoomBooking.Core.Facilities.Furniture;

/// <summary>The price of an order before it is placed.</summary>
public sealed record FurnitureQuote(decimal Base, decimal Discount, decimal HandlingFee, decimal Total);
