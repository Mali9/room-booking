namespace RoomBooking.Core.Facilities.Labs;

/// <summary>The price of an order before it is placed.</summary>
public sealed record LabsQuote(decimal Base, decimal Discount, decimal HandlingFee, decimal Total);
