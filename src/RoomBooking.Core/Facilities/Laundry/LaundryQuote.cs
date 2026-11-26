namespace RoomBooking.Core.Facilities.Laundry;

/// <summary>The price of an order before it is placed.</summary>
public sealed record LaundryQuote(decimal Base, decimal Discount, decimal HandlingFee, decimal Total);
