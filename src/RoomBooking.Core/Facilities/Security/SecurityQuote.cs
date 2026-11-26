namespace RoomBooking.Core.Facilities.Security;

/// <summary>The price of an order before it is placed.</summary>
public sealed record SecurityQuote(decimal Base, decimal Discount, decimal HandlingFee, decimal Total);
