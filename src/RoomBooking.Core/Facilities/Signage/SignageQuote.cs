namespace RoomBooking.Core.Facilities.Signage;

/// <summary>The price of an order before it is placed.</summary>
public sealed record SignageQuote(decimal Base, decimal Discount, decimal HandlingFee, decimal Total);
