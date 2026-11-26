namespace RoomBooking.Core.Facilities.Charging;

/// <summary>The price of an order before it is placed.</summary>
public sealed record ChargingQuote(decimal Base, decimal Discount, decimal HandlingFee, decimal Total);
