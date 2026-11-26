namespace RoomBooking.Core.Facilities.Garden;

/// <summary>The price of an order before it is placed.</summary>
public sealed record GardenQuote(decimal Base, decimal Discount, decimal HandlingFee, decimal Total);
