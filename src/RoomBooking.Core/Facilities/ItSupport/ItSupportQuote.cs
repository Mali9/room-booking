namespace RoomBooking.Core.Facilities.ItSupport;

/// <summary>The price of an order before it is placed.</summary>
public sealed record ItSupportQuote(decimal Base, decimal Discount, decimal HandlingFee, decimal Total);
