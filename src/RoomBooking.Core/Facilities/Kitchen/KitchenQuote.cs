namespace RoomBooking.Core.Facilities.Kitchen;

/// <summary>The price of an order before it is placed.</summary>
public sealed record KitchenQuote(decimal Base, decimal Discount, decimal HandlingFee, decimal Total);
