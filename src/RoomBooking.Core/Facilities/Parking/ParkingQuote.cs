namespace RoomBooking.Core.Facilities.Parking;

/// <summary>The price of an order before it is placed.</summary>
public sealed record ParkingQuote(decimal Base, decimal Discount, decimal HandlingFee, decimal Total);
