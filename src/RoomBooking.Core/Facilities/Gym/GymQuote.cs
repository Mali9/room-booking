namespace RoomBooking.Core.Facilities.Gym;

/// <summary>The price of an order before it is placed.</summary>
public sealed record GymQuote(decimal Base, decimal Discount, decimal HandlingFee, decimal Total);
