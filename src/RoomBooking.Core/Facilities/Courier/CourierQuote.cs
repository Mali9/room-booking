namespace RoomBooking.Core.Facilities.Courier;

/// <summary>The price of an order before it is placed.</summary>
public sealed record CourierQuote(decimal Base, decimal Discount, decimal HandlingFee, decimal Total);
