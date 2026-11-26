namespace RoomBooking.Core.Facilities.Lockers;

/// <summary>The price of an order before it is placed.</summary>
public sealed record LockersQuote(decimal Base, decimal Discount, decimal HandlingFee, decimal Total);
